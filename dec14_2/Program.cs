using System;
using System.Collections.Generic;
using System.Linq;

namespace dec14_2
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeRecipes(9, "51589");
            MakeRecipes(5, "01245");
            MakeRecipes(18, "92510");
            MakeRecipes(2018, "59414");
            MakeRecipes(0, "990941");
            Console.ReadLine();
        }

        static void MakeRecipes(int n, string expected)
        {
            var elves = new int[2] { 0, 1 };
            var recipes = new List<int> { 3, 7 };
            var found = false;
            var result = 0;
            while (!found)
            {
                var nrOfRecipes = recipes.Count;
                var sum = (recipes[elves[0]] + recipes[elves[1]]).ToString();
                for (int i = 0; i < sum.Length; i++)
                {
                    recipes.Add(int.Parse(sum.Substring(i, 1)));
                }
                for (int i = 0; i < elves.Length; i++)
                {
                    elves[i] = (elves[i] + 1 + recipes[elves[i]]) % recipes.Count;
                }
                for (int i = 0; i < sum.Length; i++)
                {
                    if (string.Join("", recipes.Skip(recipes.Count - expected.Length - i).Take(expected.Length)) == expected)
                    {
                        result = recipes.Count - expected.Length - i;
                        found = true;
                        break;
                    }
                }
            }
            Console.WriteLine($"After {result} recipes, the score is '{result}', expected was {n}");
        }
    }
}
