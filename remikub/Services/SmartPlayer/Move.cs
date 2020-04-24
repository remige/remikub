namespace remikub.Services.SmartPlayer
{
    using System.Collections.Generic;
    using remikub.Domain;

    public class Move
    {
        public Move(List<Card> playedCards, List<List<Card>> newBoard)
        {
            PlayedCards = playedCards;
            NewBoard = newBoard;
        }

        public List<Card> PlayedCards { get; }
        public List<List<Card>> NewBoard { get; }
    }
}
