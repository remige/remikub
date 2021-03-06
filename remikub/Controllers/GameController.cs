﻿namespace remikub.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using remikub.Domain;
    using remikub.Repository;
    using remikub.Hubs;
    using Microsoft.AspNetCore.SignalR;
    using remikub.Services;
    using System.Threading;

    [Route("/api/v1/games")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly INotifier _notifier;
        private readonly IAutomaticPlayer _automaticPlayer;

        public GameController(IGameRepository gameRepository, INotifier notifier, IAutomaticPlayer automaticPlayer)
        {
            _gameRepository = gameRepository;
            _notifier = notifier;
            _automaticPlayer = automaticPlayer;
        }

        [HttpPost]
        [Route("{name}")]
        public Guid CreateGame(string name)
        {
            var game = new Game(name);
            _gameRepository.SaveGame(game);
            return game.Id;
        }

        [HttpDelete]
        [Route("{gameId}")]
        public void DeleteGame(Guid gameId)
        {
            _gameRepository.DeleteGame(gameId);
        }

        [HttpGet]
        public List<GameResult> Games()
        {
            return _gameRepository.GetGames().Select(x => new GameResult(x)).ToList();
        }
        public class GameResult
        {
            public GameResult(Game game)
            {
                Id = game.Id;
                Name = game.Name;
            }

            public Guid Id { get; }
            public string Name { get; }
        }

        [HttpGet]
        [Route("{id}/users")]
        public ActionResult<ICollection<string>> GetUsers(Guid id)
        {
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            return Ok(game.UserHands.Keys);
        }

        [HttpPut]
        [Route("{id}/users/{userName}")]
        public ActionResult AddUser(Guid id, string userName)
        {
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            game.RegisterUser(userName);
            _gameRepository.SaveGame(game);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/users/bot")]
        public ActionResult AddBot(Guid id)
        {
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }

            game.RegisterUser(botsName.FirstOrDefault(x => !game.Users.Contains(x)) ?? Guid.NewGuid().ToString(), true);
            _gameRepository.SaveGame(game);
            return Ok();
        }
        private static HashSet<string> botsName = new HashSet<string> { "BogossDu93", "LicorneCeleste", "Brandon", "LordOfTheBoloss" };

        [HttpGet]
        [Route("{id}/current-user")]
        public ActionResult<List<List<Card>>> GetCurrentUser(Guid id)
        {
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            return Ok(game.CurrentUser);
        }

        [HttpGet]
        [Route("{id}/board")]
        public ActionResult<List<List<Card>>> GetBoard(Guid id)
        {
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            return Ok(game.Board);
        }

        [HttpGet]
        [Route("{id}/hand/{user}")]
        public ActionResult<List<Card>> GetHand(Guid id, string user)
        {
            // TODO : this is absolutly not secured, should be done with authentication
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            if (!game.UserHands.ContainsKey(user))
            {
                return NotFound(user);
            }

            return game.UserHands[user];
        }


        [HttpPut]
        [Route("{id}/hand/{user}/draw-card")]
        public async Task<ActionResult> DrawCard(Guid id, string user, [FromBody] List<Card> hand)
        {
            // TODO : this is absolutly not secured, should be done with authentication
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }
            if (!game.UserHands.ContainsKey(user))
            {
                return NotFound(user);
            }

            game.ReorganizeHand(user, hand);
            game.DrawCard(user);

            await EndTurn(game, user);
            return Ok();
        }


        [HttpPut]
        [Route("{id}/play/{user}")]
        public async Task<ActionResult> Play(Guid id, string user, [FromBody] PlayCommand command)
        {
            // TODO : this is absolutly not secured, should be done with authentication
            var game = _gameRepository.GetGame(id);
            if (game is null)
            {
                return NotFound(id);
            }

            game.Play(user, command.Board, command.Hand);

            await EndTurn(game, user);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/play/{user}/auto-play")]
        public async Task<ActionResult> Resolve(Guid id, string user)
        {
            var game = _gameRepository.GetGame(id);
            while (game.Winner is null)
            {
                if (game is null)
                {
                    return NotFound(id);
                }

                _automaticPlayer.AutoPlay(game, user);

                await EndTurn(game, user);

                await Task.Delay(500);

                game = _gameRepository.GetGame(id);

            }
            return Ok();
        }

        public static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        [HttpPut]
        [Route("{id}/play/{user}/auto")]
        public async Task<ActionResult> PlayAuto(Guid id, string user)
        {
            // TODO : this is absolutly not secured, should be done with authentication

            await semaphoreSlim.WaitAsync();
            try { 
                var game = _gameRepository.GetGame(id);
                if (game is null)
                {
                    return NotFound(id);
                }

                _automaticPlayer.AutoPlay(game, user);

                await EndTurn(game, user);
                return Ok();
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }
        }
        
        private async Task EndTurn(Game game, string user)
        {
            game.EndTurn();

            // Should be done by event sourcing...
            await _notifier.NotifyUserHasPlayed(game.Id, user);
            if (!string.IsNullOrEmpty(game.Winner))
            { 
                await _notifier.NotifyUserHasWon(game.Id, game.Winner);
            }

            if(game.IsBot(game.CurrentUser!))
            {
                PlayAuto(game.Id, game.CurrentUser);
            }
        }

        public class PlayCommand
        {
            public List<List<Card>> Board { get; set; } = new List<List<Card>>();
            public List<Card> Hand { get; set; } = new List<Card>();
        }
    }
}
