namespace remikub.Services
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static IEnumerable<IEnumerable<T>> GetCombinaisons<T>(this IEnumerable<T> source, int combinaisonCount)
        {
            if(combinaisonCount == 1)
            {
                return source.Select(x => new List<T> { x });
            }

            var list = new List<IEnumerable<T>>();
            foreach (var row in source)
            {
                var rowList = new List<T> { row };
                foreach (var subCombinaison in GetCombinaisons(source.Except(rowList), combinaisonCount - 1))
                {
                    list.Add(subCombinaison.Union(rowList));
                }
            }
            return list;
        }
    }
}
