using System;
using System.Linq;

namespace dec02_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var ids = System.IO.File.ReadAllLines("input.txt");
            var exactTwo = 0;
            var exactThree = 0;
            foreach (var id in ids)
            {
                if (HasExactly(id, 2))
                {
                    exactTwo++;
                }
                if (HasExactly(id, 3))
                {
                    exactThree++;
                }
            }
            Console.WriteLine(exactTwo * exactThree);
            Console.ReadLine();
        }

        static bool HasExactly(string id, int n)
        {
            foreach (var letter in id)
            {
                if (id.Where(c => c == letter).Count() == n)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
