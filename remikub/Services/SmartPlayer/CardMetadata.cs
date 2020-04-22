namespace remikub.Services.SmartPlayer
{
    using System.Collections.Generic;
    using remikub.Domain;

    public class CardMetadata
    {
        public CardMetadata(CardValue cardValue)
        {
            CardValue = cardValue;
        }
        public CardValue CardValue { get; }

        public List<Card> Cards { get; } = new List<Card>();
        public List<List<CardValue>> Combinations { get; private set; } = new List<List<CardValue>>();

        public void AddCombination(List<CardValue> combination)
        {
            Combinations.Add(combination);
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }


    }
}
