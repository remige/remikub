namespace remikub.Domain
{
    using System.Collections.Generic;

    public static class CombinationExtensions
    {
        public static bool IsValidBoard(this List<List<Card>> combinations)
        {
            foreach(var combination in combinations)
            {
                if(!combination.IsValidCombination())
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidCombination(this List<Card> combination)
        {
            if (combination.Count < 3)
            {
                return false;
            }

            return combination.IsValidSet() || combination.IsValidFlush();
        }

        private static bool IsValidSet(this List<Card> combination)
        {
            var colors = new HashSet<CardColor>();
            var values = new HashSet<int>();
            combination.ForEach(x =>
            {
                colors.Add(x.Color);
                values.Add(x.Value);
            });
            return colors.Count == combination.Count && values.Count == 1;
        }

        private static bool IsValidFlush(this List<Card> combination)
        {
            CardColor? currentColor = null;
            int? currentValue = null;
            foreach (var card in combination)
            {
                if (currentValue != null && currentValue + 1 != card.Value ||
                    currentColor != null && currentColor != card.Color)
                {
                    return false;
                }
                currentColor = card.Color;
                currentValue = card.Value;
            }
            return true;
        }
    }
}
