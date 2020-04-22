namespace remikub.Services.SmartPlayer
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using remikub.Domain;

    public class SmartPlayer : IAutomaticPlayer
    {
        private static CardValueComparer _cardValueComparer = new CardValueComparer();
        public void AutoPlay(Game game, string user)
        {
            List<Card>? handCombination;
            while((handCombination = GetHandValidCombination(game, user)) != null)
            {
                var newBoard = new List<List<Card>>(game.Board);
                newBoard.Add(handCombination);
                game.Play(user, newBoard, game.UserHands[user].Except(handCombination).ToList());
            }

            var victoryBoard = Try(game.Board.SelectMany(x => x).Concat(game.UserHands[user]).ToList());
            if(victoryBoard != null)
            {
                game.Play(user, victoryBoard, new List<Card>());
            }

            if(handCombination is null && victoryBoard is null)
            {
                game.DrawCard(user);
            }
        }

        private List<Card>? GetHandValidCombination(Game game, string user)
        {
            var metadatas = BuildMetadata(game.UserHands[user]);
            return metadatas.Values.FirstOrDefault(x => x.Combinations.Any())?
                .Combinations.First().Select(x => metadatas[x.Key].Cards.First()).ToList(); ;
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
        }

        public static List<List<Card>>? Try(List<Card> cards)
        {
            var metadatas = BuildMetadata(cards);
            if(metadatas.Any(x => x.Value.Combinations.Count < x.Value.Cards.Count))
            {
                return null;
            }

            foreach(var combination in metadatas.Values.OrderBy(x => x.Combinations.Count).First().Combinations)
            {
                var combinationCards = combination.Select(x => metadatas[x.Key].Cards.First()).ToList();

                var newBoard = cards.Except(combinationCards).ToList();
                if(!newBoard.Any())
                {
                    return new List<List<Card>> { combinationCards };
                }
                var nextCombinations = Try(newBoard);
                if(nextCombinations != null)
                {
                    nextCombinations.Add(combinationCards);
                    return nextCombinations;
                }
            }

            return null;
        }

        public static bool IsSolvable(List<Card> cards)
        {
            var metadatas = BuildMetadata(cards).Values.OrderBy(x => x.Combinations.Count);
            return metadatas.Any(x => x.Combinations.Count >= x.Cards.Count);
        }

        public static IDictionary<string, CardMetadata> BuildMetadata(List<Card> cards)
        {
            var metadatas = new Dictionary<string, CardMetadata>();
            foreach(var card in cards)
            {
                var cardValue = new CardValue(card);
                if (!metadatas.ContainsKey(cardValue.Key))
                {
                    metadatas.Add(cardValue.Key, new CardMetadata(cardValue));
                }
                metadatas[cardValue.Key].AddCard(card);
            }


            foreach (var combination in GetAllValidCombinations(cards))
            {
                foreach (var cardValue in combination)
                {
                    metadatas[cardValue.Key].AddCombination(combination);
                }
            }
            return metadatas;
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
