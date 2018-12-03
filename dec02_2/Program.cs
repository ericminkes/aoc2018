using System;
using System.Linq;

namespace dec02_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var ids = System.IO.File.ReadAllLines("input.txt");

            for (int i = 0; i < ids.Count(); i++)
            {
                for (int j = i + 1; j < ids.Count(); j++)
                {
                    var diffs = NrOfDiffs(ids[i], ids[j]);
                    if (diffs == 1)
                    {
                        Console.WriteLine(ids[i]);
                        Console.WriteLine(ids[j]);
                        for (int k = 0; k < ids[i].Length; k++)
                        {
                            if (ids[i][k] == ids[j][k])
                            {
                                Console.Write(ids[i][k]);
                            }
                        }
                        Console.WriteLine();
                        Console.ReadLine();
                        break;
                    }
                }
            }
        }

        static int NrOfDiffs(string id1, string id2)
        {
            var result = 0;
            for (int i = 0; i < id1.Length; i++)
            {
                result += (id1[i] != id2[i] ? 1 : 0);
            }
            return result;
        }
    }
}
