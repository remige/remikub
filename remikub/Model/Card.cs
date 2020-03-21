using System;

namespace remikub.Model
{
    public class Card
    {
        public Card(int number, CardColor color, int combinationId, int rank)
        {
            Number = number;
            Color = color;
            CombinationId = combinationId;
            Rank = rank;
        }
        public int Number { get; }
        public CardColor Color { get; }
        public int CombinationId { get; }
        public int Rank { get; }
    }

    public enum CardColor
    {
        Red,
        Blue,
        Black,
        Orange
    }
}
