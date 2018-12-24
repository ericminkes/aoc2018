using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace dec20_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = new List<(string Filename, int Expected)>
            {
                ("testinput1.txt", 3),
                ("testinput2.txt", 10),
                ("testinput3.txt", 18),
                ("testinput4.txt", 23),
                ("testinput5.txt", 31),
                ("input.txt", 0),
            };

            foreach (var input in inputs)
            {
                var actual = FindRoom(input.Filename);
                Console.WriteLine($"File {input.Filename}, most doors to pass: {actual}, expected {input.Expected}.");
            }
            Console.ReadLine();
        }

        class Room { public int X { get; set; } public int Y { get; set; } public bool[] Doors = new bool[4]; }

        static int FindRoom(string filename)
        {
            var grid = new Room[200,200];
            var regex = System.IO.File.ReadAllText(filename);
            var branches = ParseRegex(regex.Substring(1, regex.Length - 2));

            var x = 100; var y = 100;
            grid[x, y] = new Room { X = x, Y = y };
            foreach (var branch in branches)
            {
                TracePath(grid, branch, x, y);
            }
            var steps = new Dictionary<Room, int>();
            var q = new Queue<(Room, int)>();
            q.Enqueue((grid[x,y],0));
            while (q.Any())
            {
                (var room, var distance) = q.Dequeue();
                if (steps.ContainsKey(room))
                    continue;
                steps.Add(room, distance);
                if (room.Doors[0] && !steps.ContainsKey(grid[room.X, room.Y - 1]))
                    q.Enqueue((grid[room.X, room.Y - 1], distance + 1));
                if (room.Doors[1] && !steps.ContainsKey(grid[room.X + 1, room.Y]))
                    q.Enqueue((grid[room.X + 1, room.Y], distance + 1));
                if (room.Doors[2] && !steps.ContainsKey(grid[room.X, room.Y + 1]))
                    q.Enqueue((grid[room.X, room.Y + 1], distance + 1));
                if (room.Doors[3] && !steps.ContainsKey(grid[room.X - 1, room.Y]))
                    q.Enqueue((grid[room.X - 1, room.Y], distance + 1));
            }
            var farthestRoom = steps.First(s => s.Value == steps.Max(kvp => kvp.Value)).Key;
            Console.WriteLine($"{farthestRoom.X}, {farthestRoom.Y}");
            return steps.Max(kvp => kvp.Value);
        }

        static void TracePath(Room[,] grid, Branch branch, int x, int y)
        {
            foreach (var entry in branch.Entries)
            {
                switch (entry)
                {
                    case Leaf leaf:
                        switch (leaf.Direction)
                        {
                            case 'N':
                                grid[x, y].Doors[0] = true;
                                y = y - 1;
                                if (grid[x, y] == null)
                                    grid[x, y] = new Room { X = x, Y = y };
                                grid[x, y].Doors[2] = true;
                                break;
                            case 'E':
                                grid[x, y].Doors[1] = true;
                                x = x + 1;
                                if (grid[x, y] == null)
                                    grid[x, y] = new Room { X = x, Y = y };
                                grid[x, y].Doors[3] = true;
                                break;
                            case 'S':
                                grid[x, y].Doors[2] = true;
                                y = y + 1;
                                if (grid[x, y] == null)
                                    grid[x, y] = new Room { X = x, Y = y };
                                grid[x, y].Doors[0] = true;
                                break;
                            case 'W':
                                grid[x, y].Doors[3] = true;
                                x = x - 1;
                                if (grid[x, y] == null)
                                    grid[x, y] = new Room { X = x, Y = y };
                                grid[x, y].Doors[1] = true;
                                break;
                        }
                        break;
                    case Composite composite:
                        foreach (var subbranch in composite.Branches)
                        {
                            TracePath(grid, subbranch, x, y);
                        }
                        break;
                }
            }
        }

        abstract class Entry { }
        class Leaf : Entry { public char Direction { get; set; } }
        class Composite: Entry { public List<Branch> Branches { get; set; } = new List<Branch>(); }
        class Branch { public List<Entry> Entries { get; set; } = new List<Entry>(); }

        static List<Branch> ParseRegex(string regex)
        {
            var result = new List<Branch>();
            var current = new Branch();
            while (regex.Length > 0)
            {
                switch (regex[0])
                {
                    case '(':
                        var paren = 1;
                        var right = 0;
                        while (paren > 0)
                        {
                            right++;
                            if (regex[right] == ')')
                            {
                                paren--;
                            }
                            if (regex[right] == '(')
                            {
                                paren++;
                            }
                        }
                        current.Entries.Add(new Composite { Branches = ParseRegex(regex.Substring(1, right - 1)) });
                        regex = regex.Substring(right + 1);
                        break;
                    case '|':
                        result.Add(current);
                        current = new Branch();
                        regex = regex.Substring(1);
                        break;
                    default:
                        current.Entries.Add(new Leaf { Direction = regex[0] });
                        regex = regex.Substring(1);
                        break;
                }
            }
            result.Add(current);
            return result;
        }
    }
}
