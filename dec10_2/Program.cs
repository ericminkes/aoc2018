using System;
using System.Collections.Generic;
using System.Linq;

namespace dec10_2
{
    class Program
    {
        class Point
        {
            public int X;
            public int Y;
            public int dX;
            public int dY;
        }

        static void Main(string[] args)
        {
            var points = new List<Point>();
            foreach (var line in System.IO.File.ReadAllLines("input.txt"))
            {
                var parts = line.Split("> velocity=<");
                var parts1 = parts[0].Split(',');
                var x = int.Parse(parts1[0].Substring("position=<".Length));
                var y = int.Parse(parts1[1]);
                var parts2 = parts[1].Split(',');
                var dx = int.Parse(parts2[0]);
                var dy = int.Parse(parts2[1].Substring(0, parts2[1].Length - 1));
                var point = new Point { X = x, Y = y, dX = dx, dY = dy };
                points.Add(point);
            }
            var minBound = points.Max(p => p.X) - points.Min(p => p.X) + points.Max(p => p.Y) - points.Min(p => p.Y);
            for (int t = 1; t < 1000000; t++)
            {
                foreach (var point in points)
                {
                    point.X += point.dX;
                    point.Y += point.dY;
                }
                var bound = points.Max(p => p.X) - points.Min(p => p.X) + points.Max(p => p.Y) - points.Min(p => p.Y);
                if (bound < minBound)
                {
                    minBound = bound;
                }
                else
                {
                    foreach (var point in points)
                    {
                        point.X -= point.dX;
                        point.Y -= point.dY;
                    }
                    for (int y = points.Min(p => p.Y); y <= points.Max(p => p.Y); y++)
                    {
                        for (int x = points.Min(p => p.X); x <= points.Max(p => p.X); x++)
                        {
                            Console.Write(points.Any(p => p.X == x && p.Y == y) ? "#" : ".");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(t - 1);
                    break;
                }
            }
            Console.ReadLine();
        }
    }
}
