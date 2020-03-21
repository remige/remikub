using System;

namespace remikub.Model
{
    public class Card
    {
        public Card(int number, CardColor color, int combinationId, int position)
        {
            Number = number;
            Color = color;
            CombinationId = combinationId;
            Position = position;
        }
        public int Number { get; }
        public CardColor Color { get; }
        public int CombinationId { get; }
        public int Position { get; }
    }

    public enum CardColor
    {
        Red,
        Blue,
        Black,
        Orange
    }
}
