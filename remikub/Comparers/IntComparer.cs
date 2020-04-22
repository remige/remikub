namespace remikub.Comparers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class IntComparer : IEqualityComparer<int>
    {
        public bool Equals([AllowNull] int x, [AllowNull] int y) => x == y;

        public int GetHashCode([DisallowNull] int obj) => obj.GetHashCode();
    }
}
