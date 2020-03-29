namespace remikub.Domain
{
    public class Card
    {
        public Card(int value, CardColor color)
        {
            Value = value;
            Color = color;
        }
        public int Value { get; }
        public CardColor Color { get; }

        public bool IsEquivalent(Card card) => Value == card.Value && Color == card.Color;
    }

    public enum CardColor
    {
        Red,
        Blue,
        Black,
        Orange
    }
}
