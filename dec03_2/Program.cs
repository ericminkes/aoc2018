using System;
using System.Collections.Generic;

namespace dec03_2
{
    class Program
    {
        class Rectangle
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
            public int Claim;
        }

        static void Main(string[] args)
        {
            var rectangles = new List<Rectangle>();
            var lines = System.IO.File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var claim = int.Parse(parts[0].Substring(1));
                var coords = parts[2].Split(',');
                var lengths = parts[3].Split('x');
                var left = int.Parse(coords[0]);
                var top = int.Parse(coords[1].Substring(0, coords[1].Length - 1));
                var width = int.Parse(lengths[0]);
                var height = int.Parse(lengths[1]);

                rectangles.Add(new Rectangle
                {
                    Left = left,
                    Top = top,
                    Width = width,
                    Height = height,
                    Claim = claim
                });
            }
            for (var i = 0; i < lines.Length; i++)
            {
                int overlaps = 0;
                for (var j = 0; j < lines.Length; j++)
                {
                    if (i != j)
                    {
                        overlaps += (Overlap(rectangles[i], rectangles[j]) ? 1 : 0);
                    }
                }
                if (overlaps == 0)
                {
                    Console.WriteLine(rectangles[i].Claim);
                    break;
                }
            }
            Console.ReadLine();
        }

        static bool Overlap(Rectangle r1, Rectangle r2)
        {
            if ((r1.Left + r1.Width < r2.Left) || (r2.Left + r2.Width < r1.Left))
            {
                return false;
            }
            if ((r1.Top + r1.Height < r2.Top) || (r2.Top + r2.Height < r1.Top))
            {
                return false;
            }
            return true;
        }
    }
}
