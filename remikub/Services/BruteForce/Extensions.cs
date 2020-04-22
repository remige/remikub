namespace remikub.Services.BruteForce
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using remikub.Comparers;

    public static class Extensions
    {

        /// <summary>
        /// Returns the list of all the possible combinations (sort is ignored)
        /// For example, with ({1,2,3}, 2) =>  { {1,2}, {1, 3}, {2, 3} }
        /// </summary>
        public static HashSet<List<T>>? GetAllCombinations<T>(this List<T> source, int combinationSize, IEqualityComparer<T> comparer, Func<List<T>, bool> hasToStop)
        {
            if (combinationSize == 0)
            {
                return new HashSet<List<T>>();
            }
            if (combinationSize == 1)
            {
                return source.Select(x => new List<T> { x }).ToHashSet();
            }
            if (source.Count == combinationSize)
            {
                if (hasToStop(source))
                {
                    return null;
                }
                return new HashSet<List<T>> { source };
            }

            var result = new HashSet<List<T>>(new ListComparer<T>(comparer));

            var firstRow = source.First();
            var sourceWithoutFirstRow = source.Skip(1).ToList();
            var sub1 = sourceWithoutFirstRow.GetAllCombinations(combinationSize, comparer, hasToStop);
            var sub2 = sourceWithoutFirstRow.GetAllCombinations(combinationSize - 1, comparer, hasToStop);

            if (sub1 is null && sub2 is null)
            {
                return null;
            }
            if (sub1 != null)
            {
                result.UnionWith(sub1);
            }
            if (sub2 != null)
            {
                foreach(var sub in sub2)
                {
                    var newList =  new List<T> { firstRow }.Concat(sub).ToList();
                    if(!hasToStop(newList))
                    {
                        result.Add(newList);
                    }
                }
            }
            if (result.Any())
            {
                return result;
            }
            return null;

            /*

            var result = new HashSet<List<T>>(new ListComparer<T>(comparer));
            foreach (var group in GetAvailableGroups(source.Count, combinationSize))
            {
                var combination = new List<T>();
                for (int i = 0; i < group.Length; i++)
                {
                    if (group[i] == 1)
                    {
                        combination.Add(source[i]);
                    }
                }
                result.Add(combination);
            }
            return result;
            */
        }

        public const int MinCombinationSize = 3;
        public const int MaxCombinationSize = 5;

        public static Dictionary<int, HashSet<List<int>>> Split(this int maxValue)
        {
            var intListComparer = new ListComparer<int>(new IntComparer());

            var splitsByTotal = new Dictionary<int, HashSet<List<int>>>()
            {
                { MinCombinationSize , new HashSet<List<int>> { new List<int> { MinCombinationSize } } }
            };

            for (int i = MinCombinationSize + 1; i <= maxValue; i++)
            {
                var splits = new HashSet<List<int>>(intListComparer);
                if (i % MinCombinationSize == 0)
                {
                    // For example : 9 => (3,3,3); 12 => (3,3,3,3)
                    var split = new List<int>();
                    for (int j = 0; j < i / MinCombinationSize; j++) { split.Add(MinCombinationSize); }
                    splits.Add(split);
                }

                foreach (var parentSplits in splitsByTotal[i - 1].Where(x => !x.Any(value => value >= MaxCombinationSize)))
                {
                    foreach (var split in parentSplits.GenerateNewListForEachRowWithPlusOne())
                    {
                        splits.Add(split);
                    }
                }

                splitsByTotal.Add(i, splits);
            }

            return splitsByTotal;
        }

        private static List<List<int>> GenerateNewListForEachRowWithPlusOne(this List<int> list)
        {
            var results = list.Select(x => new List<int>(list)).ToList();
            for (int i = 0; i < results.Count; i++)
            {
                results[i][i]++;
            }
            return results;
        }

        //// NEEDS TO REFACTo THIS SHIT
        public static List<int[]> GetAvailableGroups(int totalSize, int groupSize)
        {
            if (AvailableGroupsCache.TryGetValue((totalSize, groupSize), out var groups))
            {
                return groups;
            }
            var availableGroups = GetAvailableGroupsNotCached(totalSize, groupSize);
            AvailableGroupsCache.Add((totalSize, groupSize), availableGroups);
            return availableGroups;
        }

        private static IDictionary<(int, int), List<int[]>> AvailableGroupsCache = new Dictionary<(int, int), List<int[]>>();
        private static List<int[]> GetAvailableGroupsNotCached(int totalSize, int groupSize)
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

        private static int[] CreateArray(int totalSize, int defaultValue)
        {
            var array = new int[totalSize];
            Array.Fill(array, defaultValue);
            return array;
        }
    }
}
