namespace remikub.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using remikub.Domain;
    using remikub.Repository;

    [Route("/api/v1/games")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpPost]
        [Route("{name}")]
        public Guid CreateGame(string name)
        {
            var game = new Game(name);
            _gameRepository.SaveGame(game);
            return game.Id;
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
    }
}
