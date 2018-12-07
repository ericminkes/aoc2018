using System;
using System.Linq;

namespace dec07_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = System.IO.File.ReadAllLines("input.txt")
                .Select(l => l.Split(' '))
                .Select(l => new { In = l[1], Out = l[7] })
                .ToList();

            instructions = instructions.Concat(instructions.Select(i => i.Out).Where(c => !instructions.Select(i => i.In).Contains(c))
                .Distinct().Select(c => new { In = c, Out = "" })).ToList();

            var order = "";
            while (instructions.Any())
            {
                var step = instructions.Select(i => i.In).Where(c => !instructions.Select(i => i.Out).Contains(c))
                    .OrderBy(c => c).First();
                order += step;
                instructions = instructions.Where(i => i.In != step).ToList();
            }
            Console.WriteLine(order);
            Console.ReadLine();
        }
    }
}
