namespace remikub.Repository
{
    using System;
    using System.Collections.Generic;
    using remikub.Domain;

    public interface IGameRepository
    {
        Game? GetGame(Guid id);
        List<Game> GetGames();
        void SaveGame(Game game);
    }
}
