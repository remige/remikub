using System;

namespace remikub.Model
{
    public class Card
    {
        public Card(int number, CardColor color, Coordinates coordinates)
        {
            Number = number;
            Color = color;
            Coordinates = coordinates;
        }
        public int Number { get; }
        public CardColor Color { get; }
        public Coordinates Coordinates { get;  }

    }

    public class Coordinates
    {
        public Coordinates(int combinationId, int rank)
        {
            CombinationId = combinationId;
            Rank = rank;
        }

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
