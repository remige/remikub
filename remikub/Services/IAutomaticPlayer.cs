namespace remikub.Services
{
    using remikub.Domain;

    public interface IAutomaticPlayer
    {
        public void AutoPlay(Game game, string user);
    }
}
