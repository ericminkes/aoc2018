using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace dec15_1
{
    enum UnitType { Elf, Goblin }
    class Input { public string Filename; public int Rounds; public int Hitpoints; public int Score; public UnitType Winner; }
    class Point { public int X; public int Y; public Point(int x, int y) { X = x; Y = y; } }
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = new List<Input>
            {
                new Input { Filename = "debuginput.txt" },
                new Input { Filename = "example.txt", Rounds = 47, Hitpoints = 590, Score = 27730, Winner = UnitType.Goblin },
                new Input { Filename = "testinput1.txt", Rounds = 37, Hitpoints = 982, Score = 36334, Winner = UnitType.Elf },
                new Input { Filename = "testinput2.txt", Rounds = 46, Hitpoints = 859, Score = 39514, Winner = UnitType.Elf },
                new Input { Filename = "testinput3.txt", Rounds = 35, Hitpoints = 793, Score = 27755, Winner = UnitType.Goblin },
                new Input { Filename = "testinput4.txt", Rounds = 54, Hitpoints = 536, Score = 28944, Winner = UnitType.Goblin },
                new Input { Filename = "testinput5.txt", Rounds = 20, Hitpoints = 937, Score = 18740, Winner = UnitType.Goblin },
                new Input { Filename = "input.txt" },
            };

            foreach (var input in inputs)
            {
                var result = Combat(input.Filename);
                Console.WriteLine($"Result for '{input.Filename}': score = {result.Score} ({input.Score}), turns = {result.Rounds} ({input.Rounds}), hitpoints = {result.Hitpoints} ({input.Hitpoints}), winner = {result.Winner} ({input.Winner})");
            }
            Console.ReadLine();
        }

        static (int Rounds, int Hitpoints, int Score, UnitType Winner) Combat(string filename)
        {
            (var grid, var units) = ParseInput(filename);
            var fullRounds = 0;
            while (true)
            {
                foreach (var unit in units.OrderBy(u => u.Y).ThenBy(u => u.X).ToList())
                {
                    if (unit.Hitpoints > 0)
                    {
                        if (!units.Any(u => u.UnitType == UnitType.Elf))
                        {
                            var hitpoints = units.Sum(u => u.Hitpoints);
                            return (fullRounds, hitpoints, fullRounds * hitpoints, UnitType.Goblin);
                        }
                        if (!units.Any(u => u.UnitType == UnitType.Goblin))
                        {
                            var hitpoints = units.Sum(u => u.Hitpoints);
                            return (fullRounds, hitpoints, fullRounds * hitpoints, UnitType.Elf);
                        }
                        var target = FindBestAttackTarget(unit, units, grid);
                        if (target == null)
                        {
                            var point = FindBestMoveTarget(unit, units, grid);
                            if (point != null)
                            {
                                grid[unit.X, unit.Y] = GridType.Empty;
                                unit.X = point.X;
                                unit.Y = point.Y;
                                grid[unit.X, unit.Y] = unit.UnitType == UnitType.Elf ? GridType.Elf : GridType.Goblin;
                                target = FindBestAttackTarget(unit, units, grid);
                            }
                        }
                        if (target != null)
                        {
                            target.Hitpoints -= 3;
                            if (target.Hitpoints <= 0)
                            {
                                units.Remove(target);
                                grid[target.X, target.Y] = GridType.Empty;
                            }
                        }
                    }
                }
                fullRounds++;
            }
        }

        static Point FindBestMoveTarget(Unit unit, List<Unit> units, GridType[,] grid)
        {
            var possibleTargets = units
                .Where(u => u.UnitType != unit.UnitType)
                .SelectMany(u => GetAvailableNeighbours(grid, u.X, u.Y))
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .ToList();
            var visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            var moves = new List<(Point First, Point Current, int Steps)>
            {
                (new Point(unit.X, unit.Y - 1),new Point(unit.X, unit.Y - 1), 1),
                (new Point(unit.X - 1, unit.Y),new Point(unit.X - 1, unit.Y), 1),
                (new Point(unit.X + 1, unit.Y),new Point(unit.X + 1, unit.Y), 1),
                (new Point(unit.X, unit.Y + 1),new Point(unit.X, unit.Y + 1), 1)
            };
            var possibleMoves = new List<(Point First, int Steps, Point Target)>();
            while (moves.Any())
            {
                var move = moves.First();
                moves.RemoveAt(0);
                if (visited[move.Current.X, move.Current.Y] || grid[move.Current.X, move.Current.Y] != GridType.Empty)
                    continue;
                if (possibleMoves.Any() && possibleMoves.First().Steps < move.Steps)
                    continue;
                visited[move.Current.X, move.Current.Y] = true;
                var targetHit = possibleTargets.FirstOrDefault(t => t.X == move.Current.X && t.Y == move.Current.Y);
                if (targetHit != null)
                {
                    possibleMoves.Add((move.First, move.Steps, targetHit));
                }
                moves.Add((move.First, new Point(move.Current.X, move.Current.Y - 1), move.Steps + 1));
                moves.Add((move.First, new Point(move.Current.X - 1, move.Current.Y), move.Steps + 1));
                moves.Add((move.First, new Point(move.Current.X + 1, move.Current.Y), move.Steps + 1));
                moves.Add((move.First, new Point(move.Current.X, move.Current.Y + 1), move.Steps + 1));
            }
            if (possibleMoves.Any())
            {
                foreach (var target in possibleTargets)
                {
                    foreach (var move in possibleMoves)
                    {
                        if (move.Target.X == target.X && move.Target.Y == target.Y)
                            return move.First;
                    }
                }
            }
            return null;
        }

        static IEnumerable<Point> GetAvailableNeighbours(GridType[,] grid, int x, int y)
        {
            if (grid[x, y - 1] == GridType.Empty) yield return new Point(x, y - 1);
            if (grid[x - 1, y] == GridType.Empty) yield return new Point(x - 1, y);
            if (grid[x + 1, y] == GridType.Empty) yield return new Point(x + 1, y);
            if (grid[x, y + 1] == GridType.Empty) yield return new Point(x, y + 1);
        }

        static Unit FindBestAttackTarget(Unit unit, List<Unit> units, GridType[,] grid)
        {
            Unit bestEnemy = null;
            foreach (var enemy in units.Where(u => u.UnitType != unit.UnitType).OrderBy(u => u.Y).ThenBy(u => u.X).ToList())
            {
                if ((enemy.X == unit.X && Math.Abs(enemy.Y - unit.Y) == 1)
                    || (enemy.Y == unit.Y && Math.Abs(enemy.X - unit.X) == 1))
                {
                    if (bestEnemy == null || enemy.Hitpoints < bestEnemy.Hitpoints)
                    {
                        bestEnemy = enemy;
                    }
                }
            }
            return bestEnemy;
        }

        [DebuggerDisplay("{UnitType} at ({X},{Y}) with {Hitpoints} HP")]
        class Unit { public int X; public int Y; public UnitType UnitType; public int Hitpoints; }
        enum GridType { Wall, Empty, Elf, Goblin }
        static (GridType[,] Grid, List<Unit> Units) ParseInput(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var grid = new GridType[lines[0].Length, lines.Length];
            var units = new List<Unit>();
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < lines[0].Length; x++)
                {
                    switch (line[x])
                    {
                        case '#': grid[x, y] = GridType.Wall; break;
                        case '.': grid[x, y] = GridType.Empty; break;
                        case 'E': grid[x, y] = GridType.Elf; units.Add(new Unit { X = x, Y = y, UnitType = UnitType.Elf, Hitpoints = 200 }); break;
                        case 'G': grid[x, y] = GridType.Goblin; units.Add(new Unit { X = x, Y = y, UnitType = UnitType.Goblin, Hitpoints = 200 }); break;
                    }
                }
            }
            return (grid, units);
        }
    }
}
