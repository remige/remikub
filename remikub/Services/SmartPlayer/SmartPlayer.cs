namespace remikub.Services.SmartPlayer
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using remikub.Domain;

    public class SmartPlayer : IAutomaticPlayer
    {
        private static CardValueComparer _cardValueComparer = new CardValueComparer();
        public void AutoPlay(Game game, string user)
        {
            throw new System.NotImplementedException();
        }

        public static List<List<CardValue>> GetAllValidCombinations(List<Card> cards)
        {
            var sets = cards.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Select(card => new CardValue(card)).Distinct(_cardValueComparer).ToList());
            var cardsByColor = cards.GroupBy(x => x.Color).ToDictionary(x => x.Key, x => x.Select(card => new CardValue(card)).Distinct(_cardValueComparer).ToList());

            var combinations = new List<List<CardValue>>();
            foreach (var (value, set) in sets.Where(x => x.Value.Count >= 3))
            {
                combinations.Add(set);

                // If one day, we increase the number of differnts colos need to do a more generic solution
                if (set.Count == 4)
                {
                    combinations.Add(new List<CardValue> { set[0], set[1], set[2] }); ;
                    combinations.Add(new List<CardValue> { set[0], set[1], set[3] }); ;
                    combinations.Add(new List<CardValue> { set[0], set[2], set[3] }); ;
                    combinations.Add(new List<CardValue> { set[1], set[2], set[3] }); ;
                }
            }

            foreach (var (color, singleColorCards) in cardsByColor.Where(x => x.Value.Count >= 3))
            {
                var orderedCards = singleColorCards.OrderBy(y => y.Value).ToList();

                for (int i = 0; i <= orderedCards.Count - 3; i++)
                {
                    var currentCard = orderedCards[i];
                    int j = 1;
                    CardValue nextValue;
                    List<CardValue> currentFlush = new List<CardValue> { currentCard };
                    while (i + j < orderedCards.Count && (nextValue = orderedCards[i + j]).Value == currentCard.Value + 1)
                    {
                        currentFlush.Add(nextValue);

                        if (currentFlush.Count >= 3)
                        {
                            combinations.Add(currentFlush);
                            currentFlush = new List<CardValue>(currentFlush);
                        }

                        currentCard = nextValue;
                        j++;
                    }
                }
            }
            return combinations;

            /*
var cardValues = cards.Select(x => new CardValue(x.Value, x.Color)).Distinct(new CardValueComparer()).OrderBy(x => $"{x.Value}{x.Color}").ToList();

for (int i = 0; i < cardValues.Count; i++)
{
    var currentCardValue = cardValues[i];
    var currentSet = new List<CardValue>();
    int j = 0;
    CardValue nextValue;
    while (i + j < cardValues.Count && (nextValue = cardValues[i + j]).Value == currentCardValue.Value)
    {

        currentSet.Add(nextValue);
        if (currentSet.Count >= 3)
        {
            combinations.Add(currentSet);
            currentSet = new List<CardValue>(currentSet);
        }
        if (currentSet.Count == 4)
        {
            combinations.Add(new List<CardValue> { currentSet[0], currentSet[2], currentSet[3] }  );
            combinations.Add(new List<CardValue> { currentSet[0], currentSet[1], currentSet[3] });
        }
        j++;
    }
 }
 */
        }

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
        }

        public class CardValueComparer : IEqualityComparer<CardValue>
        {
            public bool Equals([AllowNull] CardValue x, [AllowNull] CardValue y)
            {
                if (x is null && y is null) { return true; }
                if (x is null || y is null) { return false; }
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode([DisallowNull] CardValue obj) => $"{obj.Value}{obj.Color}".GetHashCode();
        }
    }
}
