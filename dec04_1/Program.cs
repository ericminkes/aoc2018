using System;
using System.Collections.Generic;
using System.Linq;

namespace dec04_1
{
    class SleepPeriod
    {
        public int Guard;
        public int StartMinute;
        public int EndMinute;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt").OrderBy(line => line).ToList();
            var currentGuard = -1;
            var currentStartMinute = -1;
            var periods = new List<SleepPeriod>();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                switch (parts[2].ToLower())
                {
                    case "guard": currentGuard = int.Parse(parts[3].Substring(1)); break;
                    case "falls": currentStartMinute = int.Parse(parts[1].Substring(3, 2)); break;
                    case "wakes":
                        var endMinute = int.Parse(parts[1].Substring(3,2));
                        periods.Add(new SleepPeriod { Guard = currentGuard, StartMinute = currentStartMinute, EndMinute = endMinute });
                        break;
                }
            }

            var guardMostAsleep = periods
                .GroupBy(p => p.Guard)
                .Select(pg => new { Guard = pg.Key, TotalTime = pg.Sum(sp => sp.EndMinute - sp.StartMinute)})
                .OrderByDescending(g => g.TotalTime)
                .First()
                .Guard;
            var minutes = new int[60];
            foreach (var period in periods.Where(p => p.Guard == guardMostAsleep))
            {
                for (int i = period.StartMinute; i < period.EndMinute; i++)
                {
                    minutes[i]++;
                }
            }
            var currentMax = -1;
            var currentIndex = -1;
            for (int i = 0; i < 60; i++)
            {
                if (minutes[i] > currentMax)
                {
                    currentMax = minutes[i];
                    currentIndex = i;
                }
            }
            Console.WriteLine($"Guard {guardMostAsleep}, minute {currentIndex}, answer {guardMostAsleep * currentIndex}");
            Console.ReadLine();
        }
    }
}
