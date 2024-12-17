using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day17
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "17");
            var input = InputOutputHelper.GetInput(false, "17");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var registers = InitializeRegisters(input);
            var program = ParseProgram(input[4]);

            var output = ExecuteProgram(registers, program);

            InputOutputHelper.WriteOutput(isTest, string.Join(",", output));
        }

        private static long[] InitializeRegisters(string[] input)
        {
            return new long[]
            {
                ParseRegister(input[0]), 
                ParseRegister(input[1]), 
                ParseRegister(input[2]) 
            };
        }

        private static int ParseRegister(string input)
        {
            return int.Parse(input.Split(" ")[2]);
        }

        private static List<long> ParseProgram(string input)
        {
            var program = new List<long>();
            var parts = input.Split(" ")[1].Split(',');

            foreach (var part in parts)
            {
                program.Add(int.Parse(part));
            }

            return program;
        }

        private static List<long> ExecuteProgram(long[] registers, List<long> program)
        {
            var output = new List<long>();
            long instructionPointer = 0;

            while (instructionPointer < program.Count)
            {
                long opcode = program[(int)instructionPointer];
                long operand = program[(int)instructionPointer + 1];

                switch (opcode)
                {
                    case 0:
                        int divisor0 = (int)Math.Pow(2, GetComboOperandValue(registers, operand));
                        if (divisor0 != 0)
                        {
                            registers[0] /= divisor0;
                        }
                        break;
                    case 1:
                        registers[1] ^= operand;
                        break;
                    case 2:
                        registers[1] = GetComboOperandValue(registers, operand) % 8;
                        break;
                    case 3:
                        if (registers[0] != 0)
                        {
                            instructionPointer = operand;
                            continue;
                        }
                        break;
                    case 4:
                        registers[1] ^= registers[2];
                        break;
                    case 5:
                        output.Add(GetComboOperandValue(registers, operand) % 8);
                        break;
                    case 6:
                        int divisor6 = (int)Math.Pow(2, GetComboOperandValue(registers, operand));
                        if (divisor6 != 0)
                        {
                            registers[1] = registers[0] / divisor6;
                        }
                        break;
                    case 7:
                        int divisor7 = (int)Math.Pow(2, GetComboOperandValue(registers, operand));
                        if (divisor7 != 0)
                        {
                            registers[2] = registers[0] / divisor7;
                        }
                        break;
                }

                instructionPointer += 2;
            }

            return output;
        }

        private static long GetComboOperandValue(long[] registers, long operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registers[0],
                5 => registers[1],
                6 => registers[2],
                _ => throw new ArgumentException("Invalid combo operand")
            };
        }
        
        private static void PartTwo(bool isTest, string[] input)
        {
            var registers = InitializeRegisters(input);
            var program = ParseProgram(input[4]).Select(x => (long)x).ToList();
            var lowerBound = 0;
            
            long result = 0;
            for (long i = lowerBound; i < long.MaxValue; i += 8)
            {
                var newProgram = ExecuteProgram(new long[] { i, 0, 0 }, program);
                if (newProgram.SequenceEqual(program))
                {
                    result = i;
                    break;
                }
            }

            InputOutputHelper.WriteOutput(isTest, result);
        }
    }
}