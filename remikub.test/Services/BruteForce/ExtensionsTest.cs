namespace remikub.test.Services.BruteForce
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using NUnit.Framework;
    using remikub.Comparers;
    using remikub.Services.BruteForce;

    public class ExtensionsTest
    {
        [Test]
        public void GetAllCombinations_Should_Returns_All_Combinations()
        {
            new List<int> { 1, 2, 3 }.GetAllCombinations(2, new IntComparer(), (List<int> l) => false).Should().BeEquivalentTo(
                    new List<List<int>> { new List<int> { 1, 2 }, new List<int> { 2, 3 }, new List<int> { 1, 3 } }
            );
        }

        [Test]
        public void GetAllCombinations_Should_Ignore_Duplicates()
        {
            new List<int> { 1, 1, 3 }.GetAllCombinations(2, new IntComparer(), (List<int> l) => false).Should().BeEquivalentTo(
                    new List<List<int>> { new List<int> { 1, 1 }, new List<int> { 1, 3 } }
            );
        }

        [Test]
        public void GetAllCombinations_Sould_Retruns_Data_()
        {
            var list = new List<int>();
            for (int i = 0; i < 100; i++) { list.Add(i); }
            var combinations = list.GetAllCombinations(3, new IntComparer(), (List<int> l) => false);
            foreach (var combination in combinations)
            {
                combination.Count.Should().Be(3);
            }
        }

        [Test]
        public void Split_Should_Returns_All_PossibleSplits()
        {
            var splitBySize = 10.Split();
            splitBySize[3].Should().BeEquivalentTo(new HashSet<List<int>> { new List<int> { 3 } });
            splitBySize[4].Should().BeEquivalentTo(new HashSet<List<int>> { new List<int> { 4 } });
            splitBySize[5].Should().BeEquivalentTo(new HashSet<List<int>> { new List<int> { 5 } });
            splitBySize[6].Should().BeEquivalentTo(new HashSet<List<int>> { new List<int> { 6 }, new List<int> { 3, 3 } });
            splitBySize[7].Should().BeEquivalentTo(new HashSet<List<int>> { new List<int> { 7 }, new List<int> { 3, 4 } });
        }
    }
}
