using Kga.Algo.Trees.Trie;
using System;

namespace Kga.Algo.Trees.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            string[] words = {
                "hello","help","hell","hill","holla",
                "hitler","bit","hate","heat","cold",
                "cope","hole","haven", "hater", "bat",
                "holder", "folded", "fail", "hail", "bella"
            };

            var set = new TrieSet(words);

            set.Remove("hail");
            set.Add("tiger");

            Console.WriteLine("No. of nodes in tree: " + set.Size);
            Console.WriteLine("No. of words in tree: " + set.KeyCount);
            Console.WriteLine("No. of hits on tree: " + set.HitCount);

            Console.WriteLine();

            foreach (var result in set)
            {
                Console.WriteLine(result);
            }

            Console.WriteLine();

            var map = new TrieMap<string>
            {
                {"book", "Something you read."},
                {"holiday", "Time for relaxation."},
                {"bowl", "Where you store stuff or eat from."}
            };

            var bible = map["book"] + " Or maybe write one.";

            map.Remove("book");

            map.Add("bible", bible);

            //map["love"] = "404 not found."; // ArgumentException

            Console.WriteLine("No. of nodes in tree: " + map.Size);
            Console.WriteLine("No. of words in tree: " + map.KeyCount);
            Console.WriteLine("No. of hits on tree: " + map.HitCount);

            Console.WriteLine();

            foreach (var result in map.Search("b"))
            {
                Console.WriteLine(result.Key + " - " + result.Value);
            }

            Console.ReadLine();
        }
    }

}
