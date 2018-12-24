using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace dec17_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Testinput can contain {ComputeCapacity("testinput.txt")} (expected 29).");
            Console.WriteLine($"Input can contain {ComputeCapacity("input.txt")}.");
            Console.ReadLine();
        }

        enum Grid { Sand = 0, Clay = 1, Still = 2, Flow = 3 }
        static int ComputeCapacity(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var coordinates = new List<Point>();
            foreach (var line in lines)
            {
                var parts = line.Split(", ");
                var isX = parts[0][0] == 'x';
                var n = int.Parse(parts[0].Substring(2));
                var range = parts[1].Substring(2).Split("..");
                var begin = int.Parse(range[0]);
                var end = int.Parse(range[1]);
                if (isX)
                {
                    for (int y = begin; y <= end; y++)
                    {
                        coordinates.Add(new Point(n, y));
                    }
                }
                else
                {
                    for (int x = begin; x <= end; x++)
                    {
                        coordinates.Add(new Point(x, n));
                    }
                }
            }

            var minX = coordinates.Min(p => p.X);
            var maxX = coordinates.Max(p => p.X);
            var minY = coordinates.Min(p => p.Y);
            var maxY = coordinates.Max(p => p.Y);
            var grid = new Grid[maxX + 1 - (minX - 1), maxY + 1];
            foreach (var coordinate in coordinates)
            {
                grid[coordinate.X - (minX - 1), coordinate.Y] = Grid.Clay;
            }
            FillGrid(grid, DetermineReservoirs(grid), 500 - (minX - 1), 0);

            if (filename == "testinput.txt")
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = 0; x < maxX + 1 - (minX - 1); x++)
                        Console.Write(grid[x, y] == Grid.Sand ? "." : grid[x, y] == Grid.Clay ? "#" : grid[x, y] == Grid.Still ? "~" : "|");
                    Console.WriteLine();
                }
            }
            var total = 0;
            for (int x = 0; x < maxX + 1 - (minX - 1); x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    total += (grid[x, y] == Grid.Still) ? 1 : 0;
                }
            }
            return total;
        }

        static void FillGrid(Grid[,] grid, bool[,] reservoirs, int x, int y)
        {
            if (x < 0 || x >= grid.GetLength(0))
                return;
            if (y >= grid.GetLength(1))
                return;
            if (grid[x, y] == Grid.Clay)
                return;
            if (reservoirs[x, y])
            {
                if (grid[x, y] != Grid.Still)
                {
                    grid[x, y] = Grid.Still;
                    FillGrid(grid, reservoirs, x - 1, y);
                    FillGrid(grid, reservoirs, x + 1, y);
                    FillGrid(grid, reservoirs, x, y + 1);
                }
                return;
            }
            grid[x, y] = Grid.Flow;
            if (y < grid.GetLength(1) - 1)
            {
                FillGrid(grid, reservoirs, x, y + 1);
                if (grid[x, y + 1] == Grid.Clay || reservoirs[x, y + 1])
                {
                    if (grid[x - 1, y] == Grid.Sand)
                        FillGrid(grid, reservoirs, x - 1, y);
                    if (grid[x + 1, y] == Grid.Sand)
                        FillGrid(grid, reservoirs, x + 1, y);
                }
            }
        }

        static bool[,] DetermineReservoirs(Grid[,] grid)
        {
            var reservoirs = new bool[grid.GetLength(0), grid.GetLength(1)];
            for (int y = grid.GetLength(1) - 2; y >= 0; y--)
            {
                for (int x = 1; x < grid.GetLength(0) - 1; x++)
                {
                    if (grid[x, y] == Grid.Sand)
                    {
                        if (grid[x - 1, y] == Grid.Sand)
                        {
                            reservoirs[x, y] = reservoirs[x - 1, y];
                        }
                        else
                        {
                            var right = -1;
                            for (int x2 = x + 1; x2 < grid.GetLength(0); x2++)
                            {
                                if (grid[x2, y] == Grid.Clay)
                                {
                                    right = x2;
                                    break;
                                }
                            }
                            if (right != -1)
                            {
                                var isReservoir = true;
                                for (int x2 = x; x2 < right; x2++)
                                {
                                    if (grid[x2, y + 1] == Grid.Sand && !reservoirs[x2, y + 1])
                                    {
                                        isReservoir = false;
                                        break;
                                    }
                                }
                                reservoirs[x, y] = isReservoir;
                            }
                        }
                    }
                }
            }
            return reservoirs;
        }
    }
}
