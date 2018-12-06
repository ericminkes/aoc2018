using System;
using System.Linq;
using System.Text;

namespace dec05_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymer = Encoding.ASCII.GetBytes(System.IO.File.ReadAllText("input.txt"));

            var types = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var min = int.MaxValue;
            foreach (var t in types)
            {
                var result = RemoveAndReduce(polymer, t);
                if (result < min)
                {
                    min = result;
                }
            }
            Console.WriteLine(min);
            Console.ReadLine();
        }

        static int RemoveAndReduce(byte[] polymer, byte t)
        {
            var polymerWithoutType = polymer.Where(c => c != t && c != t + 32).ToArray();
            var reactions = 0;
            do
            {
                (reactions, polymerWithoutType) = Reduce(polymerWithoutType);
            } while (reactions > 0);
            return polymerWithoutType.Length;
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
                    polymer[i + 1] = 0;
                    i++;
                    reactions++;
                }
            }
            return (reactions, polymer.Where(c => c != 0).ToArray());
        }
    }
}
