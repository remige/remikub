namespace remikub.Controllers
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

    [Route("/api/v1/games")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly INotifier _notifier;

        public GameController(IGameRepository gameRepository, INotifier notifier )
        {
            _gameRepository = gameRepository;
            _notifier = notifier;
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
        public ActionResult<ICollection<string>> AddUser(Guid id)
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
        public async Task<ActionResult> DrawCard(Guid id, string user)
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

            game.DrawCard(user);

            // Shoudl be done by event sourcing...
            await _notifier.NotifyUserHasPlayed(id, user);
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
            if (!game.UserHands.ContainsKey(user))
            {
                return NotFound(user);
            }

            game.Play(user, command.Board, command.Hand);

            // Shoudl be done by event sourcing...
            await _notifier.NotifyUserHasPlayed(id, user);
            return Ok();
        }

        public class PlayCommand
        {
            public List<List<Card>> Board { get; set; } = new List<List<Card>>();
            public List<Card> Hand { get; set; } = new List<Card>();
        }
    }
}
