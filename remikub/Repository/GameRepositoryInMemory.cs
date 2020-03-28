namespace remikub.Repository
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using remikub.Domain;

    public class GameRepositoryInMemory : IGameRepository
    {
        private readonly IDictionary<Guid, Game> _games = new ConcurrentDictionary<Guid, Game>(); 

        public Game? GetGame(Guid id)
        {
            if(_games.TryGetValue(id, out var game))
            {
                return game;
            }
            return null;
        }

        public List<Game> GetGames() => _games.Values.ToList();

        public void SaveGame(Game game)
        {
            _games[game.Id] = game;
        }
    }
}
