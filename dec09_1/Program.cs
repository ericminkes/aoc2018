using System;
using System.Diagnostics;
using System.Linq;

namespace dec09_1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var line in System.IO.File.ReadAllLines("input.txt").Select(l => l.Split(' ')))
            {
                Console.WriteLine(PlayGame(int.Parse(line[0]), int.Parse(line[6])));
            }
            Console.ReadLine();
        }

        [DebuggerDisplay("{Value}")]
        class Marble
        {
            public int Value;
            public Marble Next;
            public Marble Prev;
        }

        static int PlayGame(int nrOfPlayers, int nrOfMarbles)
        {
            var current = new Marble { Value = 0 };
            current.Next = current;
            current.Prev = current;
            var players = new int[nrOfPlayers];
            var player = 0;
            for (int m = 1; m <= nrOfMarbles; m++)
            {
                if (m % 23 == 0)
                {
                    players[player] += m;
                    for (int i = 0; i < 7; i++)
                    {
                        current = current.Prev;
                    }
                    current.Prev.Next = current.Next;
                    current.Next.Prev = current.Prev;
                    players[player] += current.Value;
                    current = current.Next;
                }
                else
                {
                    current = current.Next;
                    var marble = new Marble { Value = m };
                    marble.Next = current.Next;
                    marble.Prev = current;
                    current.Next.Prev = marble;
                    current.Next = marble;
                    current = marble;
                }
                player = (player + 1) % nrOfPlayers;
            }
            return players.Max();
        }
    }
}
