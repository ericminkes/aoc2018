using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace dec13_1
{
    class Program
    {
        enum State { Left, Straight, Right }
        enum Direction { Up, Right, Down, Left }
        [DebuggerDisplay("({X}, {Y}) - {Direction} - {State}")]
        class Cart { public int X; public int Y; public Direction Direction; public State State; }

        static void Main(string[] args)
        {
            foreach (var filename in new[] { "testinput.txt", "input.txt" })
            {
                (var grid, var carts) = ParseInput(filename);
                (var x, var y) = FirstCrash(grid, carts);
                Console.WriteLine($"'{filename}' first crash at ({x},{y})");
            }
            Console.ReadLine();
        }

        static (int X, int Y) FirstCrash(string[] grid, List<Cart> carts)
        {
            for (int tick = 0; ; tick++)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList())
                {
                    cart.X = cart.X + (cart.Direction == Direction.Left ? -1 : cart.Direction == Direction.Right ? 1 : 0);
                    cart.Y = cart.Y + (cart.Direction == Direction.Up ? -1 : cart.Direction == Direction.Down ? 1 : 0);
                    switch (grid[cart.Y][cart.X])
                    {
                        case '/':
                            switch (cart.Direction)
                            {
                                case Direction.Up: cart.Direction = Direction.Right; break;
                                case Direction.Right: cart.Direction = Direction.Up; break;
                                case Direction.Down: cart.Direction = Direction.Left; break;
                                case Direction.Left: cart.Direction = Direction.Down; break;
                            }
                            break;
                        case '\\':
                            switch (cart.Direction)
                            {
                                case Direction.Up: cart.Direction = Direction.Left; break;
                                case Direction.Left: cart.Direction = Direction.Up; break;
                                case Direction.Down: cart.Direction = Direction.Right; break;
                                case Direction.Right: cart.Direction = Direction.Down; break;
                            }
                            break;
                        case '+':
                            switch (cart.State)
                            {
                                case State.Left:
                                    cart.Direction = TurnLeft(cart.Direction);
                                    cart.State = State.Straight;
                                    break;
                                case State.Straight:
                                    cart.State = State.Right;
                                    break;
                                case State.Right:
                                    cart.Direction = TurnRight(cart.Direction);
                                    cart.State = State.Left;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                    var crash = carts.GroupBy(c => new {c.X, c.Y }).FirstOrDefault(g => g.Count() > 1);
                    if (crash != null)
                    {
                        return (crash.Key.X, crash.Key.Y);
                    }
                }
            }
        }

        static Direction TurnLeft(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Direction.Left;
                case Direction.Right: return Direction.Up;
                case Direction.Down: return Direction.Right;
                case Direction.Left: return Direction.Down;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        static Direction TurnRight(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Direction.Right;
                case Direction.Right: return Direction.Down;
                case Direction.Down: return Direction.Left;
                case Direction.Left: return Direction.Up;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        static (string[] Grid, List<Cart> Carts) ParseInput(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var carts = new List<Cart>();
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if ("<>v^".Contains(lines[y][x]))
                    {
                        var cart = new Cart { X = x, Y = y, Direction = GetDirection(lines[y][x]), State = State.Left };
                        carts.Add(cart);
                    }
                }
            }
            return (lines, carts);
        }

        static Direction GetDirection(char c)
        {
            switch (c)
            {
                case '<': return Direction.Left;
                case '>': return Direction.Right;
                case 'v': return Direction.Down;
                case '^': return Direction.Up;
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
