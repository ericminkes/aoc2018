using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace reddit
{
    class Program
    {
        //copied from reddit
        static void Main(string[] args)
        {
            HashSet<int> currentPlants = new HashSet<int>();
            Dictionary<int, bool> plantRules = new Dictionary<int, bool>();
            StreamReader file = new StreamReader("input.txt");

            string line = file.ReadLine();
            line.Skip(15).Select((x, i) => new { x, i }).Where(c => c.x == '#').Select(c => c.i).ToList().ForEach(x => currentPlants.Add(x));
            line = file.ReadLine();
            while (!file.EndOfStream)
            {
                line = file.ReadLine();
                int binary = line.Take(5).Select((x, i) => new { x, i }).Where(c => c.x == '#').Sum(c => (int)Math.Pow(2, c.i));
                plantRules.Add(binary, line[9] == '#' ? true : false);
            }

            long iterations = 50000000000;
            long totalSum = 0;
            HashSet<int> newPlants = new HashSet<int>();

            for (int iter = 1; iter <= iterations; iter++)
            {
                newPlants = new HashSet<int>();
                int min = currentPlants.Min() - 3;
                int max = currentPlants.Max() + 3;

                for (int pot = min; pot <= max; pot++)
                {
                    int sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (currentPlants.Contains(pot + i - 2)) sum += (int)Math.Pow(2, i);
                    }
                    if (plantRules[sum]) newPlants.Add(pot);
                }
                // the simulation converged to a stable point
                if (currentPlants.Select(x => x + 1).Except(newPlants).Count() == 0)
                {
                    currentPlants = newPlants;
                    totalSum = currentPlants.Sum();
                    totalSum += currentPlants.Count() * (iterations - iter);
                    break;
                }

                currentPlants = newPlants;
            }

            Console.WriteLine(totalSum);
            Console.ReadLine();
        }
    }
}

//using System;
//using System.Collections.Generic;

//namespace dec12_2
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            ProcessInput("input.txt");
//            Console.ReadLine();
//        }

//        static void ProcessInput(string filename)
//        {
//            var lines = System.IO.File.ReadAllLines(filename);
//            var state = lines[0].Substring("initial state: ".Length);
//            var pots = new bool[1000000];
//            for (int i = 0; i < state.Length; i++)
//            {
//                pots[i + 500000] = state[i] == '#';
//            }
//            var patterns = new Dictionary<int, bool>();
//            for (int i = 0; i < 32; i++)
//            {
//                patterns.Add(i, false);
//            }
//            for (int i = 0; i < lines.Length - 2; i++)
//            {
//                var parts = lines[i+2].Split(' ');
//                var index = 0;
//                var p2 = 16;
//                for (int j = 0; j < 5; j++)
//                {
//                    if (parts[0][j] == '#')
//                    {
//                        index += p2;
//                    }
//                    p2 /= 2;
//                }
//                patterns[index] = parts[2] == "#";
//            }

//            var sums = new long[10001];
//            for (long gen = 1; gen <= 10000; gen++)
//            {
//                var newGen = new bool[1000000];
//                for (var i = 500000 - gen * 2; i < 500000 + 20 + gen * 2; i++)
//                {
//                    var index = 0;
//                    var p2 =16;
//                    for (int j = -2; j <= 2; j++)
//                    {
//                        if (pots[i + j])
//                        {
//                            index += p2;
//                        }
//                        p2 /= 2;
//                    }
//                    newGen[i] = patterns[index];
//                }
//                pots = newGen;
//                long sum = 0;
//                for (var i = 500000 - gen * 2; i < 500000 + 20 + gen * 2; i++)
//                    sum += pots[i] ? (i - 500000) : 0;
//                sums[gen] = sum;
//            }
//            var diff = sums[10000] - sums[9999];
//            //var total = sums[9999];
//            //for (long gen = 10000; gen < 50000000000; gen++)
//            //{
//            //    total += diff;
//            //}
//            //Console.WriteLine(total);
//            Console.WriteLine(sums[10000]);
//            Console.WriteLine(50000000000 - 10000);
//            Console.WriteLine(diff);
//            Console.WriteLine(diff * (50000000000 - 10000));
//            Console.WriteLine(sums[10000] + diff * (50000000000 - 10000));
//        }
//    }
//}
