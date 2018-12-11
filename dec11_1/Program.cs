using System;

namespace dec11_1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var serial in new[] { 18, 42, 9995 })
                DetermineMaxLevel(serial);
            Console.ReadLine();
        }

        private static void DetermineMaxLevel(int serial)
        {
            var grid = new int[301,301];
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    long rackID = x + 10;
                    long power = (rackID * y + serial) * rackID;
                    int hundredsDigit = ((int)(power % 1000)) / 100;
                    grid[x, y] = hundredsDigit - 5;
                }
            }
            var max = long.MinValue;
            var maxX = -1; var maxY = -1;
            for (int x = 1; x <= 298; x++)
            {
                for (int y = 1; y <= 298; y++)
                {
                    var level = 0;
                    for (int i = 0; i <= 2; i++)
                        for (int j = 0; j <= 2; j++)
                            level += grid[x + i, y + j];
                    if (level > max)
                    {
                        max = level;
                        maxX = x;
                        maxY = y;
                    }
                }
            }
            Console.WriteLine($"For serial {serial}: Level {max} at {maxX}, {maxY}");
        }
    }
}
