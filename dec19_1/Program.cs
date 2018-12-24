using System;
using System.Linq;

namespace dec19_1
{
    class Program
    {
        class Instruction { public string Opcode; public ulong Op1; public ulong Op2; public ulong Op3; }
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            var ipReg = int.Parse(lines[0].Substring(4));
            var instr = lines.Skip(1).Select(l =>
            {
                var parts = l.Split(' ');
                return new Instruction { Opcode = parts[0], Op1 = ulong.Parse(parts[1]), Op2 = ulong.Parse(parts[2]), Op3 = ulong.Parse(parts[3]) };
            }).ToList();

            ulong ip = 0;
            var state = new ulong[6];
            while (ip >= 0 && ip < (ulong)instr.LongCount())
            {
                var current = instr[(int)ip];
                state[ipReg] = ip;
                Execute(current.Opcode, state, new ulong[] { 0L, current.Op1, current.Op2, current.Op3 });
                ip = state[ipReg];
                ip++;
            }
            Console.WriteLine($"Value of register 0 is {state[0]}.");
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
    }
}
