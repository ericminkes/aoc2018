using System;
using System.Collections.Generic;
using System.Linq;

namespace dec01_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var changes = System.IO.File.ReadAllLines("input.txt").Select(l => int.Parse(l)).ToList();
            var frequencies = new List<int> { 0 };
            var current = 0;
            var found = false;
            while (!found)
            {
                foreach (var change in changes)
                {
                    current += change;
                    if (frequencies.Contains(current))
                    {
                        Console.WriteLine(current);
                        found = true;
                        break;
                    }
                    else
                    {
                        frequencies.Add(current);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
