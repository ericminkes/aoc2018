using System;
using System.Collections.Generic;

namespace dec12_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput("testinput.txt");
            ProcessInput("input.txt");
            Console.ReadLine();
        }

        static void ProcessInput(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var state = lines[0].Substring("initial state: ".Length);
            var pots = new bool[10000];
            for (int i = 0; i < state.Length; i++)
            {
                pots[i + 5000] = state[i] == '#';
            }
            var patterns = new Dictionary<int, bool>();
            for (int i = 0; i < 32; i++)
            {
                patterns.Add(i, false);
            }
            for (int i = 0; i < lines.Length - 2; i++)
            {
                var parts = lines[i+2].Split(' ');
                var index = 0;
                var p2 = 16;
                for (int j = 0; j < 5; j++)
                {
                    if (parts[0][j] == '#')
                    {
                        index += p2;
                    }
                    p2 /= 2;
                }
                patterns[index] = parts[2] == "#";
            }

            for (var gen = 0; gen < 20; gen++)
            {
                var newGen = new bool[10000];
                for (var i = 2; i < 9998; i++)
                {
                    var index = 0;
                    var p2 =16;
                    for (int j = -2; j <= 2; j++)
                    {
                        if (pots[i + j])
                        {
                            index += p2;
                        }
                        p2 /= 2;
                    }
                    newGen[i] = patterns[index];
                }
                pots = newGen;
            }
            var sum = 0;
            for (int i = 0; i < 10000; i++)
                sum += pots[i] ? (i - 5000) : 0;
            Console.WriteLine(sum);
        }
    }
}
