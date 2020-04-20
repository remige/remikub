namespace remikub.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using remikub.Domain;
    using remikub.Services.BruteForce;

    public class BruteForcePlayer : IAutomaticPlayer
    {
        public void AutoPlay(Game game, string user)
        {
            for (int i = Math.Min(3, game.UserHands[user].Count); i > 0; i--)
            {
                var move = Play(game, user, i);
                if (move != null)
                {
                    game.Play(user, move.NewBoard, game.UserHands[user].Except(move.PlayedCards).ToList());
                    return;
                }
            }
            game.DrawCard(user);
        }

        private Move? Play(Game game, string user, int tryNbCard)
        {
            foreach (var cardsToPlay in GetAvailableCombinations(game.UserHands[user], tryNbCard))
            {
                var allCards = new List<Card>(game.Board.SelectMany(x => x).Union(cardsToPlay).ToList());

                if (allCards.Count < CombinationDisposition.MinCombinationSize)
                {
                    return null;
                }
                var boardDispositions = CombinationDisposition.ComputeBoardDisposition(allCards.Count)[allCards.Count];

                foreach (var disposition in boardDispositions)
                {
                    var newBoard = CombineCards(new List<Card>(allCards), disposition.CombinationSizes);
                    if (newBoard != null)
                    {
                        return new Move(cardsToPlay, newBoard);
                    }
                }
            }
            return null;
        }

        private IDictionary<string, List<List<Card>>?> CombineCardCache = new Dictionary<string, List<List<Card>>?>();
        public List<List<Card>>? CombineCards(List<Card> cards, List<int> combinationSizes)
        {
            var callKey = string.Join("-", cards.Select(x => $"{x.Color}_{x.Value}").OrderBy(x => x)) + "__" + string.Join("-", combinationSizes.OrderBy(x => x));
            if (CombineCardCache.TryGetValue(callKey, out var result))
            {
                return result;
            }
            var combinedCards = CombineCardsWithoutCache(cards, combinationSizes);
            CombineCardCache.Add(callKey, combinedCards);
            return combinedCards;
        }

        private List<List<Card>>? CombineCardsWithoutCache(List<Card> cards, List<int> combinationSizes)
        {
            var firstSize = combinationSizes.First();

            var validCombinations = GetAvailableCombinations(cards, firstSize);
            if (!validCombinations.Any())
            {
                return null;
            }

            var newSizes = combinationSizes.Skip(1).ToList();
            foreach (var combination in validCombinations)
            {
                if (!newSizes.Any())
                {
                    return new List<List<Card>> { combination };
                }

                var availableCombinations = CombineCards(cards.Except(combination).ToList(), newSizes);
                if (availableCombinations != null)
                {
                    availableCombinations.Add(combination);
                    return availableCombinations;
                }
            }
            return null;
        }

        private IDictionary<(int, int), List<int[]>> AvailableGroupsCache = new ConcurrentDictionary<(int, int), List<int[]>>();

        public List<List<Card>> GetAvailableCombinations(List<Card> cards, int combinationSize)
        {
            var cardsWithoutDuplicates = new Dictionary<string, Card>();
            foreach (var card in cards)
            {
                var cardKey = $"{card.Color}${card.Value}";
                if (!cardsWithoutDuplicates.ContainsKey(cardKey))
                {
                    cardsWithoutDuplicates.Add(cardKey, card);
                }
            }
            var uniqueCards = cardsWithoutDuplicates.Values.ToArray();
            var combinations = new List<List<Card>>();
            foreach (var group in GetAvailableGroups(uniqueCards.Length, combinationSize))
            {
                var combination = new List<Card>();
                for (int i = 0; i < group.Length; i++)
                {
                    if (group[i] == 1)
                    {
                        combination.Add(uniqueCards[i]);
                    }
                }
                combination = combination.OrderBy(x => x.Value).ToList();
                if (combination.IsValidCombination())
                {
                    combinations.Add(combination);
                }
            }
            return combinations;
        }
        public List<int[]> GetAvailableGroups(int totalSize, int groupSize)
        {
            if (AvailableGroupsCache.TryGetValue((totalSize, groupSize), out var groups))
            {
                return groups;
            }
            var availableGroups = GetAvailableGroupsNotCached(totalSize, groupSize);
            AvailableGroupsCache.Add((totalSize, groupSize), availableGroups);
            return availableGroups;
        }
        private List<int[]> GetAvailableGroupsNotCached(int totalSize, int groupSize)
        {
            if (groupSize == 0)
            {
                return new List<int[]> { CreateArray(totalSize, 0) };
            }
            if (groupSize == 1)
            {
                var list = new List<int[]>();
                for (int i = 0; i < totalSize; i++)
                {
                    var group = new int[totalSize];
                    group[i] = 1;
                    list.Add(group);
                }

                return list;
            }

            if (totalSize == groupSize)
            {
                return new List<int[]> { CreateArray(totalSize, 1) };
            }

            var result = new List<int[]>();

            var groupForZero = new int[] { 0 };
            foreach (var group in GetAvailableGroups(totalSize - 1, groupSize))
            {
                result.Add(groupForZero.Concat(group).ToArray());
            }

            var groupForOne = new int[] { 1 };
            foreach (var group in GetAvailableGroups(totalSize - 1, groupSize - 1))
            {
                result.Add(groupForOne.Concat(group).ToArray());
            }

            return result;
        }

        private int[] CreateArray(int totalSize, int defaultValue)
        {
            var array = new int[totalSize];
            Array.Fill(array, defaultValue);
            return array;
        }
    }
}
