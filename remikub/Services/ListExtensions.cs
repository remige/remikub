namespace remikub.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using remikub.Services.BruteForce;

    public static class ListExtensions
    {

        public static List<List<List<T>>> GetCombinaisons<T>(this List<T> source)
        {
            var result = new List<List<List<T>>> { new List<List<T>> { source } };
            if (source.Count == 1)
            {
                return result;
            }

            Parallel.ForEach(source, new ParallelOptions { MaxDegreeOfParallelism = 10 }, row =>
            {
                var subCombinations = source.Except(new List<T> { row }).ToList().GetCombinaisons();
                foreach (var subCombination in subCombinations)
                {
                    result.Add(new List<List<T>> { new List<T> { row } }.Union(subCombination).ToList());
                }
            });
            return result;
        }
    }
}
