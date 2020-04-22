namespace remikub.Services.SmartPlayer
{
    using remikub.Domain;

    public class CardValue
    {
        public CardValue(Card card)
        {
            Value = card.Value;
            Color = card.Color;
        }

        public CardValue(int value, CardColor color)
        {
            Value = value;
            Color = color;
        }

        public int Value { get; }
        public CardColor Color { get; }
        public string Key => $"{Value}{Color}";
        public override string ToString() => $"{Value}_{Color}";
    }
}
