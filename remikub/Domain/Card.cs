namespace remikub.Domain
{
    public class Card
    {
        public Card(int number, CardColor color)
        {
            Value = number;
            Color = color;
        }
        public int Value { get; }
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
