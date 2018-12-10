using System;
using System.Collections.Generic;
using System.Linq;

namespace dec08_1
{
    class Program
    {
        class Node
        {
            public List<Node> Children = new List<Node>();
            public List<int> Metadata = new List<int>();
        }

        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt").Split(' ').Select(s => int.Parse(s)).ToList();

            var index = 0;
            (var root, _) = Parse(input, index);
            var total = MetadataTotal(root);

            Console.WriteLine(total);
            Console.ReadLine();
        }

        static int MetadataTotal(Node node)
        {
            return node.Metadata.Sum() + node.Children.Select(c => MetadataTotal(c)).Sum();
        }

        static (Node, int) Parse(List<int> input, int index)
        {
            var node = new Node();
            var nrChildren = input[index++];
            var nrMetadata = input[index++];
            for (int i = 0; i < nrChildren; i++)
            {
                Node child = null;
                (child, index) = Parse(input, index);
                node.Children.Add(child);
            }
            for (int i = 0; i < nrMetadata; i++)
            {
                node.Metadata.Add(input[index++]);
            }
            return (node, index);
        }
    }
}
