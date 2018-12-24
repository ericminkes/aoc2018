using System;

namespace dec19_2
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong ip = 0;
            ulong r0 = 1; ulong r1 = 0; ulong r2 = 0; ulong r3 = 0; ulong r4 = 0; ulong r5 = 0;
            while (ip >= 0 && ip < 36)
            {
                r2 = ip;
                switch (ip)
                {
                    case 0: r2 = r2 + 16; break; //00 addi 2 16 2 // goto 17
                    case 1:
                        for (ulong i = 1; i <= r4; i++)
                        {
                            if ((r4 % i) == 0)
                            {
                                r0 += i;
                            }
                        }
                        Console.WriteLine(r0);
                        Console.ReadLine();
                        break;
                    //case 1: r1 = 1; break; //01 seti 1 0 1
                    //case 2: r3 = 1; break; //02 seti 1 3 3
                    //case 3: r5 = r1 * r3; break;  //03 mulr 1 3 5
                    //case 4: r5 = r4 == r5 ? (ulong)1 : 0; break; //04 eqrr 5 4 5
                    //case 5: r2 = r5 + r2; break; //05 addr 5 2 2 // goto 6 / 7
                    //case 6: r2 = r2 + 1; break; //06 addi 2 1 2 // goto 8
                    //case 7: r0 = r1 + r0; break; //07 addr 1 0 0
                    //case 8: r3 = r3 + 1; break; //08 addi 3 1 3
                    //case 9: r5 = r3 > r4 ? (ulong)1 : 0; break; //09 gtrr 3 4 5
                    //case 10: r2 = r2 + r5; break; //10 addr 2 5 2 // goto 11 / 12
                    //case 11: r2 = 2; break; //11 seti 2 6 2 // goto 3
                    //case 12: r1 = r1 + 1; break; //12 addi 1 1 1
                    //case 13: r5 = r1 > r4 ? (ulong)1 : 0; break; //13 gtrr 1 4 5
                    //case 14: r2 = r5 + r2; break; //14 addr 5 2 2 // goto 15 / 16
                    //case 15: r2 = 1; break; //15 seti 1 1 2 // goto 2
                    //case 16: r2 = r2 * r2; break; //16 mulr 2 2 2  //exit
                    case 17: r4 = r4 + 2; break; //17 addi 4 2 4 // init
                    case 18: r4 = r4 * r4; break; //18 mulr 4 4 4
                    case 19: r4 = r2 * r4; break; //19 mulr 2 4 4
                    case 20: r4 = r4 * 11; break; //20 muli 4 11 4
                    case 21: r5 = r5 + 6; break; //21 addi 5 6 5
                    case 22: r5 = r5 * r2; break; //22 mulr 5 2 5
                    case 23: r5 = r5 + 19; break; //23 addi 5 19 5
                    case 24: r4 = r4 + r5; break; //24 addr 4 5 4
                    case 25: r2 = r2 + r0; break; //25 addr 2 0 2 // goto 26 (1) or 27 (2)
                    case 26: r2 = 0; break; //26 seti 0 7 2 // goto 1
                    case 27: r5 = r2; break;//27 setr 2 6 5 
                    case 28: r5 = r5 * r2; break; //28 mulr 5 2 5
                    case 29: r5 = r2 + r5; break; //29 addr 2 5 5
                    case 30: r5 = r2 * r5; break; //30 mulr 2 5 5
                    case 31: r5 = r5 * 14; break; //31 muli 5 14 5
                    case 32: r5 = r5 * r2; break; //32 mulr 5 2 5
                    case 33: r4 = r4 + r5; break; //33 addr 4 5 4
                    case 34: r0 = 0; break; //34 seti 0 7 0
                    case 35: r2 = 0; break; //35 seti 0 3 2 // goto 1
                }
                ip = r2;
                ip++;
            }
            Console.WriteLine($"Value of register 0 is {r0}.");
            Console.ReadLine();
        }
    }
}
