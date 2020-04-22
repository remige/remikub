namespace remikub.Comparers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using remikub.Domain;

    public class CardComparer : IEqualityComparer<Card>
    {
        public bool Equals([AllowNull] Card x, [AllowNull] Card y)
        {
            if (x is null && y is null)
            {
                return true;
            }
            if (x is null || y is null)
            {
                return false;
            }
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] Card card) => $"{card.Color}{card.Value}".GetHashCode();
    }
}
