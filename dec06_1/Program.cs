using System;
using System.Drawing;
using System.Linq;

namespace dec06_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = System.IO.File.ReadAllLines("input.txt").Select(l =>
            {
                var parts = l.Split(", ");
                return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
            }).Select((p, i) => new { Point = p, Index = i }).ToList();

            var grid = new int[500, 500];
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    var distances = points.Select(p => new { p.Index, Distance = ManhattanDistance(p.Point, new Point(x, y)) }).ToList();
                    var minDistance = distances.Min(d => d.Distance);
                    if (distances.Count(d => d.Distance == minDistance) > 1)
                    {
                        grid[x, y] = -1;
                    }
                    else
                    {
                        grid[x, y] = distances.Single(d => d.Distance == minDistance).Index;
                    }
                }
            }
            var maxSize = int.MinValue;
            foreach (var point in points)
            {
                var onBorder = false;
                for (int x = 0; x < 500; x++)
                {
                    if (grid[x, 0] == point.Index || grid[x, 499] == point.Index)
                        onBorder = true;
                    if (grid[0, x] == point.Index || grid[499, x] == point.Index)
                        onBorder = true;
                }
                if (!onBorder)
                {
                    var size = 0;
                    for (int x = 0; x < 500; x++)
                    {
                        for (int y = 0; y < 500; y++)
                        {
                            if (grid[x, y] == point.Index)
                                size++;
                        }
                    }
                    if (size > maxSize)
                    {
                        maxSize = size;
                    }
                }
            }
            Console.WriteLine(maxSize);
            Console.ReadLine();
        }

        static int ManhattanDistance(Point p1, Point p2)
        {
            var dx = p1.X - p2.X;
            if (dx < 0) dx = -dx;
            var dy = p1.Y - p2.Y;
            if (dy < 0) dy = -dy;
            return dx + dy;
        }
    }
}
