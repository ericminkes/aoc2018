using System;
using System.Drawing;
using System.Linq;

namespace dec06_2
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

            var grid = new bool[500, 500];
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    var distances = points.Select(p => new { p.Index, Distance = ManhattanDistance(p.Point, new Point(x, y)) }).ToList();
                    grid[x, y] = distances.Sum(d => d.Distance) < 10000;
                }
            }

            var size = 0;
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    if (grid[x, y])
                        size++;
                }
            }
            Console.WriteLine(size);
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
