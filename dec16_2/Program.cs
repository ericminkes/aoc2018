using System;
using System.Collections.Generic;
using System.Linq;

namespace dec16_2
{
    class Program
    {
        static List<string> Opcodes = new List<string> { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr"};
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input1.txt");

            var possibleOpcodes = new List<string>[16];
            for (int i = 0; i < lines.Length; i += 4)
            {
                var before = ParseState(lines[i]);
                var instruction = ParseInstructions(lines[i + 1]);
                var after = ParseState(lines[i + 2]);
                foreach (var opcode in Opcodes)
                {
                    if (IsEqual(Execute(opcode, before.Select(x => x).ToArray(), instruction), after))
                    {
                        if (possibleOpcodes[instruction[0]] == null)
                        {
                            possibleOpcodes[instruction[0]] = new List<string>();
                        }
                        if (!possibleOpcodes[instruction[0]].Contains(opcode))
                            possibleOpcodes[instruction[0]].Add(opcode);
                    }
                }
            }
            var actualOpcodes = new Dictionary<int, string>();
            while (possibleOpcodes.Any(po => po != null))
            {
                var toRemove = new List<List<string>>();
                for (int i = 0; i < 16; i++)
                {
                    var po = possibleOpcodes[i];
                    if (po != null && po.Count == 1)
                    {
                        actualOpcodes.Add(i, po.First());
                        foreach (var po2 in possibleOpcodes.Where(x => x != null && x != po))
                        {
                            if (po2.Contains(po.First()))
                                po2.Remove(po.First());
                        }
                        possibleOpcodes[i] = null;
                    }
                }
            }

            lines = System.IO.File.ReadAllLines("input2.txt");
            var state = new ulong[4];
            foreach (var line in lines)
            {
                var instr = ParseInstructions(line);
                state = Execute(actualOpcodes[(int)instr[0]], state.Select(x => x).ToArray(), instr);
            }
            Console.WriteLine(state[0]);
            Console.ReadLine();
        }

        static ulong[] Execute(string opcode, ulong[] state, ulong[] instr)
        {
            switch (opcode)
            {
                case "addr": state[instr[3]] = state[instr[1]] + state[instr[2]]; return state;
                case "addi": state[instr[3]] = state[instr[1]] + instr[2]; return state;
                case "mulr": state[instr[3]] = state[instr[1]] * state[instr[2]]; return state;
                case "muli": state[instr[3]] = state[instr[1]] * instr[2]; return state;
                case "banr": state[instr[3]] = state[instr[1]] & state[instr[2]]; return state;
                case "bani": state[instr[3]] = state[instr[1]] & instr[2]; return state;
                case "borr": state[instr[3]] = state[instr[1]] | state[instr[2]]; return state;
                case "bori": state[instr[3]] = state[instr[1]] | instr[2]; return state;
                case "setr": state[instr[3]] = state[instr[1]]; return state;
                case "seti": state[instr[3]] = instr[1]; return state;
                case "gtir": state[instr[3]] = instr[1] > state[instr[2]] ? (ulong)1 : 0; return state;
                case "gtri": state[instr[3]] = state[instr[1]] > instr[2] ? (ulong)1 : 0; return state;
                case "gtrr": state[instr[3]] = state[instr[1]] > state[instr[2]] ? (ulong)1 : 0; return state;
                case "eqir": state[instr[3]] = instr[1] == state[instr[2]] ? (ulong)1 : 0; return state;
                case "eqri": state[instr[3]] = state[instr[1]] == instr[2] ? (ulong)1 : 0; return state;
                case "eqrr": state[instr[3]] = state[instr[1]] == state[instr[2]] ? (ulong)1 : 0; return state;
                default:
                    throw new InvalidOperationException($"{opcode}");
            }
        }

        static bool IsEqual(ulong[] first, ulong[] second)
        {
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
            }
            return true;
        }

        static ulong[] ParseState(string line)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var result = new ulong[4];
            result[0] = ulong.Parse(parts[1].Substring(1, parts[1].Length - 2));
            result[1] = ulong.Parse(parts[2].Substring(0, parts[2].Length - 1));
            result[2] = ulong.Parse(parts[3].Substring(0, parts[3].Length - 1));
            result[3] = ulong.Parse(parts[4].Substring(0, parts[4].Length - 1));
            return result;
        }

        static ulong[] ParseInstructions(string line)
        {
            return line.Split(" ").Select(p => ulong.Parse(p)).ToArray();
        }
    }
}
