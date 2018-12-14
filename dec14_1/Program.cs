using System;
using System.Collections.Generic;
using System.Linq;

namespace dec14_1
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeRecipes(9, "5158916779");
            MakeRecipes(5, "0124515891");
            MakeRecipes(18, "9251071085");
            MakeRecipes(2018, "5941429882");
            MakeRecipes(990941, "");
            Console.ReadLine();
        }

        static void MakeRecipes(int n, string expected)
        {
            var elves = new int[2] { 0, 1 };
            var recipes = new List<int> { 3, 7 };
            while (recipes.Count < n + 10)
            {
                var sum = (recipes[elves[0]] + recipes[elves[1]]).ToString();
                for (int i = 0; i < sum.Length; i++)
                {
                    recipes.Add(int.Parse(sum.Substring(i, 1)));
                }
                for (int i = 0; i < elves.Length; i++)
                {
                    elves[i] = (elves[i] + 1 + recipes[elves[i]]) % recipes.Count;
                }
            }
            var result = string.Join("", recipes.Skip(n).Take(10));
            Console.WriteLine($"After {n} recipes, the score is '{result}', expected was '{expected}'");
        }
    }
}
