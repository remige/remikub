using System;

namespace remikub.Model
{
    public class Card
    {
        public Card(int number, CardColor color)
        {
            Id = Guid.NewGuid();
            Number = number;
            Color = color;
        }
        public Guid Id { get; }
        public int Number { get; }
        public CardColor Color { get; }
    }

    public enum CardColor
    {
        Red,
        Blue,
        Black,
        Orange
    }
}
