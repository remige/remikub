namespace remikub.Comparers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class ListComparer<T> : IEqualityComparer<List<T>>
    {
        private readonly IEqualityComparer<T> _comparer;
        public ListComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public bool Equals([AllowNull] List<T> x, [AllowNull] List<T> y)
        {
            if(x is null && y is null)
            {
                return true;
            }
            if(x is null || y is null)
            {
                return false;
            }
            return GetHashCode(x) == GetHashCode(y); 
        }

        public int GetHashCode([DisallowNull] List<T> obj)
            => string.Join("_", obj.Select(x => x?.GetHashCode()).OrderBy(x => x)).GetHashCode();
    }
}
