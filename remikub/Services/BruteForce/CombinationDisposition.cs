namespace remikub.Services.BruteForce
{
    using System;
    using System.Collections.Generic;

    public class CombinationDisposition
    {
        public CombinationDisposition(int combinationSize) : this(new List<int> { combinationSize }) { }
        public CombinationDisposition(List<int> combinationSizes)
        {
            if (combinationSizes is null) { throw new ArgumentNullException(nameof(combinationSizes)); }
            CombinationSizes = combinationSizes;
            CombinationSizes.Sort();
        }

        public List<int> CombinationSizes { get; }

        private const int minCombinationSize = 3;
        private const int maxCombinationSize = 13;
        public static Dictionary<int, HashSet<CombinationDisposition>> ComputeBoardDisposition(int nbCards)
        {
            var dispostionsByNbCards = new Dictionary<int, HashSet<CombinationDisposition>>()
            {
                { minCombinationSize , new HashSet<CombinationDisposition> { new CombinationDisposition(minCombinationSize) } }
            };

            for (int i = minCombinationSize + 1; i <= nbCards; i++)
            {
                var dispositions = new HashSet<CombinationDisposition>();
                if (i % minCombinationSize == 0)
                {
                    var newGroup = new List<int>();
                    for (int j = 0; j < i / minCombinationSize; j++) { newGroup.Add(minCombinationSize); }
                    dispositions.Add(new CombinationDisposition(newGroup));
                }

                foreach (var parentDispositions in dispostionsByNbCards[i - 1])
                {
                    foreach (var newDisposition in parentDispositions.GenerateNextDispositions())
                    {
                        dispositions.Add(newDisposition);
                    }
                }

                dispostionsByNbCards.Add(i, dispositions);
            }

            return dispostionsByNbCards;
        }

        public List<CombinationDisposition> GenerateNextDispositions()
        {
            var list = new List<CombinationDisposition>();

            // We create one disposition for each combination like this :
            // (3, 3) => (4, 3) (3, 4)
            // (3, 4, 5) => (4, 4, 5) (3, 5, 5) (3, 4, 6)
            for (int i = 0; i < CombinationSizes.Count; i++)
            {
                var newDisposition = new List<int>();
                bool ignore = false;
                for (int j = 0; j < CombinationSizes.Count; j++)
                {
                    if (i == j)
                    {
                        var value = CombinationSizes[j] + 1;
                        if (value > maxCombinationSize)
                        {
                            ignore = true;
                            break;
                        } else
                        {
                            newDisposition.Add(value);
                        }
                        
                    }
                    else
                    {
                        newDisposition.Add(CombinationSizes[j]);
                    }
                }

                if (!ignore) { list.Add(new CombinationDisposition(newDisposition)); }
            }
            return list;
        }

        public override bool Equals(object? value)
        {
            return value != null && value.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return string.Join("_", CombinationSizes).GetHashCode();
        }
    }
}
