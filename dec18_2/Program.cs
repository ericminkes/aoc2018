using System;

namespace dec18_2
{
    class Program
    {
        static void Main(string[] args)
        {
            //            Console.WriteLine($"Testinput has resource value {ComputeResourceValue("testinput.txt")} (expected 1147)");
            Console.WriteLine($"Input has resource value {ComputeResourceValue("input.txt")}");
            Console.ReadLine();
        }

        enum Area { Open, Trees, Lumberyard }
        static int ComputeResourceValue(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            var width = lines[0].Length;
            var height = lines.Length;
            var area = new Area[width, height];
            var totalTrees = 0; var totalYards = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    area[x, y] = lines[y][x] == '.' ? Area.Open : lines[y][x] == '|' ? Area.Trees : Area.Lumberyard;
                }
            }

            var u = 1000000000;
            while (u > 100000)
                u -= 28;
            for (int t = 0; t < u; t++)
            {
                if ((u - t) % 28 == 0)
                    Console.WriteLine($"{t + 1} minutes have passed (current trees {totalTrees}, yards {totalYards}).");
                var newArea = new Area[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var trees = 0; var opens = 0; var yards = 0;
                        for (var i = x - 1; i <= x + 1; i++)
                        {
                            for (var j = y - 1; j <= y + 1; j++)
                            {
                                if (i < 0 || i >= width || j < 0 || j >= height)
                                    continue;
                                if (i == x && j == y)
                                    continue;
                                switch (area[i, j])
                                {
                                    case Area.Trees: trees++; break;
                                    case Area.Open: opens++; break;
                                    case Area.Lumberyard: yards++; break;
                                }
                            }
                        }
                        switch (area[x, y])
                        {
                            case Area.Open: newArea[x, y] = trees >= 3 ? Area.Trees : Area.Open; break;
                            case Area.Trees: newArea[x, y] = yards >= 3 ? Area.Lumberyard : Area.Trees; break;
                            case Area.Lumberyard: newArea[x, y] = (trees >= 1 && yards >= 1) ? Area.Lumberyard : Area.Open; break;
                        }
                    }
                }
                area = newArea;

                totalTrees = 0; totalYards = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        totalTrees += (area[x, y] == Area.Trees ? 1 : 0);
                        totalYards += (area[x, y] == Area.Lumberyard ? 1 : 0);
                    }
                }
            }
            return totalTrees * totalYards;
        }
    }
}
