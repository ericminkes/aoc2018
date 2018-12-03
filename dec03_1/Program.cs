using System;
using System.Linq;

namespace dec03_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var fabric = new int[1000,1000];
            foreach (var line in System.IO.File.ReadAllLines("input.txt"))
            {
                var parts = line.Split(' ');
                var coords = parts[2].Split(',');
                var lengths = parts[3].Split('x');
                var left = int.Parse(coords[0]);
                var top = int.Parse(coords[1].Substring(0, coords[1].Length - 1));
                var width = int.Parse(lengths[0]);
                var height = int.Parse(lengths[1]);
                for (int x = left; x < left + width; x++)
                {
                    for (int y = top; y < top + height; y++)
                    {
                        fabric[x,y]++;
                    }
                }
            }
            var overlap = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    if (fabric[x,y] > 1)
                    {
                        overlap++;
                    }
                }
            }
            Console.WriteLine(overlap);
            Console.ReadLine();
        }
    }
}
