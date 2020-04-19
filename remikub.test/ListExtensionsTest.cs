namespace remikub.test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using remikub.Services;
    using remikub.Services.BruteForce;

    public class ListExtensionsTest
    {

        [Test]
        public void CombinationDisposition_Test()
        {
            var results = CombinationDisposition.ComputeBoardDisposition(200);

            using (var fs = new StreamWriter("D:/test.txt"))
            {
                foreach (var (nbCards, dispositions) in results)
                {
                    fs.Write($"Nb ${nbCards} => ");
                    foreach (var disposition in dispositions)
                    {
                        fs.Write($"/");
                        fs.Write(string.Join(",", disposition.CombinationSizes));
                    }
                    fs.Write(Environment.NewLine);
                }

            }
        }

        [Test]
        public void Test1()
        {
            var list = new List<int>();
            for(int i = 0; i < 5;  i++)
            {
                list.Add(i);
            }
            var results = list.GetCombinaisons();

            using (var fs = new StreamWriter("D:/test.txt")) {
                foreach(var solution in results)
                {
                    foreach (var combination in solution)
                    {
                        fs.Write("/");
                        fs.Write(string.Join(",", combination));
                        fs.Write("/");
                    }
                    fs.Write(Environment.NewLine);
                }

            }
            // File.WriteAllLines(", results.Select(combinationGroup => combinationGroup.Select(x => x.Select()))));
            var res = results.Count();
        }
    }
}