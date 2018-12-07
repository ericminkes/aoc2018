using System;
using System.Collections.Generic;
using System.Linq;

namespace dec07_1
{
    class Program
    {
        class Worker
        {
            public int TimeLeft;
            public int Step;
        }

        static void Main(string[] args)
        {
            var instructions = System.IO.File.ReadAllLines("input.txt")
            //var instructions = new List<string>
            //    {
            //        "Step C must be finished before step A can begin.",
            //        "Step C must be finished before step F can begin.",
            //        "Step A must be finished before step B can begin.",
            //        "Step A must be finished before step D can begin.",
            //        "Step B must be finished before step E can begin.",
            //        "Step D must be finished before step E can begin.",
            //        "Step F must be finished before step E can begin."
            //    }
                .Select(l => l.Split(' '))
                .Select(l => new { In = l[1][0] - 4, Out = l[7][0] - 4 })
                .ToList();

            instructions = instructions.Concat(instructions.Select(i => i.Out).Where(c => !instructions.Select(i => i.In).Contains(c))
                .Distinct().Select(c => new { In = c, Out = 0 })).ToList();

            var workers = new Worker[5] { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };
            for (int t = 0; ; t++)
            {
                if (instructions.Any() || workers.Any(w => w.TimeLeft > 0))
                {
                    if (workers.Any(w => w.Step == 0))
                    {
                        var options = instructions.Select(i => i.In)
                            .Where(c => !instructions.Select(i => i.Out).Contains(c))
                            .Where(c => !workers.Any(w => w.Step == c))
                            .Distinct().OrderBy(c => c).ToList();
                        while (options.Any() && workers.Any(w => w.Step == 0))
                        {
                            var option = options.First();
                            foreach (var worker in workers)
                            {
                                if (worker.TimeLeft == 0)
                                {
                                    worker.Step = option;
                                    worker.TimeLeft = option;
                                    options = options.Skip(1).ToList();
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < workers.Length; i++)
                    {
                        if (workers[i].TimeLeft > 0)
                        {
                            workers[i].TimeLeft -= 1;
                            if (workers[i].TimeLeft == 0)
                            {
                                instructions = instructions.Where(x => x.In != workers[i].Step).ToList();
                                workers[i].Step = 0;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine(t);
                    break;
                }
            }
            Console.ReadLine();
        }
    }
}
