using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BusTimetable.Generator.Generators
{
    public static class IdGenerator
    {
        private static readonly List<string> Words = new List<string>();
        private static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());
        private static readonly HashSet<string> IssuedIdentifiers = new HashSet<string>();

        static IdGenerator()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "words.txt");
            foreach (var line in File.ReadAllLines(path))
            {
                if (line.Length == 1)
                {
                    continue;
                }

                Words.AddRange(line.Split(new []{',', '.'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).Where(x => x.Length > 1));
            }
            Shuffle(Words);
        }

        public static string GetId(int number)
        {
            while (true)
            {
                var id = $"{Words[Rnd.Next(0, Words.Count)]}-{Words[Rnd.Next(0, Words.Count)]}";
                if (IssuedIdentifiers.Contains(id))
                {
                    continue;
                }

                IssuedIdentifiers.Add($"{id}-{number}");
                return id;
            }
        }


        private static void Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rnd.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
