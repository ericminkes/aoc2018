using System;

namespace dec11_2
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
            var maxSize = -1;
            var gridSums = new int[301,301,301];
            for (int size = 1; size <= 300; size++)
            {
                for (int x = 1; x <= 301 - size; x++)
                {
                    for (int y = 1; y <= 301 - size; y++)
                    {
                        if (size == 1)
                        {
                            gridSums[x, y, 1] = grid[x, y];
                        }
                        else
                        {
                            var level = gridSums[x, y, size - 1];
                            for (int i = 0; i <= size - 2; i++)
                            {
                                level += grid[x + i, y + size - 1];
                                level += grid[x + size - 1, y + i];
                            }
                            level += grid[x + size - 1, y + size - 1];
                            if (level > max)
                            {
                                max = level;
                                maxX = x;
                                maxY = y;
                                maxSize = size;
                            }
                            gridSums[x, y, size] = level;
                        }
                    }
                }
            }
            Console.WriteLine($"For serial {serial}: Level {max} at {maxX},{maxY}, size {maxSize}");
        }
    }
}
