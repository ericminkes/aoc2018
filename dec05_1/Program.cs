using System;
using System.Linq;
using System.Text;

namespace dec05_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymer = Encoding.ASCII.GetBytes(System.IO.File.ReadAllText("input.txt"));
            var reactions = 0;
            do
            {
                (reactions, polymer) = Reduce(polymer);
            } while (reactions > 0);
            Console.WriteLine(polymer.Length);
            Console.ReadLine();
        }

        static (int, byte[]) Reduce(byte[] polymer)
        {
            var reactions = 0;
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                var a = polymer[i];
                var b = polymer[i+1];
                if (a - b == 32 || b - a == 32)
                {
                    polymer[i] = 0;
                    polymer[i+1] = 0;
                    i++;
                    reactions++;
                }
            }
            return (reactions, polymer.Where(c => c != 0).ToArray());
        }
    }
}
