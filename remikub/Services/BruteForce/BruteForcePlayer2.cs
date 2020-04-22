namespace remikub.Services.BruteForce
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using remikub.Comparers;
    using remikub.Domain;

    public class BruteForcePlayer2 : IAutomaticPlayer
    {
        private static readonly IDictionary<int, HashSet<List<int>>> AvailableSplitsByTotal = 60.Split();
        private static readonly CardComparer _cardComparer = new CardComparer();

        public void AutoPlay(Game game, string user)
        {
            var hasPlayed = false;
            Move? move;
            while ((move = Play(game, user, 3)) != null)
            {
                hasPlayed = true;
                game.Play(user, move.NewBoard, game.UserHands[user].Except(move.PlayedCards).ToList());
            }

            while ((move = Play(game, user, 1)) != null)
            {
                hasPlayed = true;
                game.Play(user, move.NewBoard, game.UserHands[user].Except(move.PlayedCards).ToList());
                return;
            }

            if (!hasPlayed)
            {
                game.DrawCard(user);
            }
        }



        private Move? Play(Game game, string user, int tryNbCard)
        {
            var validCombinations = game.UserHands[user].GetAllCombinations(tryNbCard, _cardComparer, IsCombinationDestinatedToFail)?
                .Select(x => x.OrderBy(x => x.Value).ToList());

            var filtered = validCombinations?.Where(x => x.IsValidCombination(tryNbCard));
            if(validCombinations is null)
            {
                return null;
            }

            foreach (var cardsToPlay in validCombinations)
            {
                Console.WriteLine(string.Join(",", cardsToPlay.Select(x => $"{x.Value}_{x.Color}")));

                var allCards = game.Board.SelectMany(x => x).Union(cardsToPlay).ToList();
                if (allCards.Count < Extensions.MinCombinationSize)
                {
                    return null;
                }

                var splits = AvailableSplitsByTotal[allCards.Count];
                // Foreach split combination, we try to find an agencement of the card 
                foreach (var split in splits)
                {
                    var newBoard = CombineCards(allCards, split.OrderByDescending(x => x).ToList());
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

        private static bool IsCombinationDestinatedToFail(List<Card> list)
        {
            if (list.Count <= 1 || list.Count > 3)
            {
                return false;
            }
            if (list.Count == 3)
            {
                return !list.OrderBy(x => x.Value).ToList().IsValidCombination();
            }

            var isFlush = list[0].Color == list[1].Color;
            var valueDiff = Math.Abs(list[0].Value - list[1].Value);

            return (!isFlush && valueDiff != 0) ||
                    (isFlush && (valueDiff == 0 || valueDiff >= list.Count));
        }

        private List<List<Card>>? CombineCardsWithoutCache(List<Card> cards, List<int> combinationSizes)
        {
            var firstSize = combinationSizes.First();

            var cardsWithoutDuplicates = cards.Distinct().ToList();

            var validCombinations = cardsWithoutDuplicates.GetAllCombinations(firstSize, _cardComparer, IsCombinationDestinatedToFail)?
                .Where(x => x.IsValidCombination()) // Not sure this is necessary
                .ToHashSet();
            if (validCombinations is null || !validCombinations.Any())
            {
                return null;
            }

            // If multiple combinations exists, we try each of them, and continue with the next
            var sizesWithoutFirst = combinationSizes.Skip(1).ToList();
            foreach (var combination in validCombinations)
            {
                if (!sizesWithoutFirst.Any())
                {
                    // We reach the end of the three
                    return new List<List<Card>> { combination };
                }

                var availableCombinations = CombineCards(cardsWithoutDuplicates.Except(combination).ToList(), sizesWithoutFirst);
                if (availableCombinations != null)
                {
                    availableCombinations.Add(combination);
                    return availableCombinations;
                }
            }
            return null;
        }
    }
}
