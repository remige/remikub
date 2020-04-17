namespace remikub.Hubs
{
    using System;

    public class UserHasWon
    {
        public UserHasWon(Guid gameId, string user)
        {
            GameId = gameId;
            User = user;
        }
        public Guid GameId { get; }
        public string User { get; }
    }
}
