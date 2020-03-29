namespace remikub.Hubs
{
    using System;

    public class UserHasPlayed
    {
        public UserHasPlayed(Guid gameId, string user)
        {
            GameId = gameId;
            User = user;
        }
        public Guid GameId { get; }
        public string User { get; }
    }
}
