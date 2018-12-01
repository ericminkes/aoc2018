using System;
using System.Linq;

namespace dec01_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            Console.WriteLine(lines.Select(l => int.Parse(l)).Sum(i => i));
            Console.ReadLine();
        }
    }
}
