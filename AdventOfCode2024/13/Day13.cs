using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day13
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "13");
            var input = InputOutputHelper.GetInput(false, "13");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var result = 0;

            List<ClawMachine> clawMachines = ParseInput(input);
            foreach (var clawMachine in clawMachines)
            {
                var (pressesA, pressesB) = clawMachine.GetFewestButtonPresses();
                if (pressesA >= 0 && pressesB >= 0)
                {
                    result += pressesA * 3 + pressesB;
                }
            }

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static List<ClawMachine> ParseInput(string[] input)
        {
            var clawMachines = new List<ClawMachine>();
            for (int i = 0; i < input.Length; i += 4)
            {
                var clawMachine = ParseClawMachine(string.Join("\n", input.Skip(i).Take(3)));
                clawMachines.Add(clawMachine);
            }
            return clawMachines;
        }

        private static ClawMachine ParseClawMachine(string input)
        {
            var parts = input.Split(new[] { "Button A: X+", ", Y+", "Button B: X+", "Prize: X=", ", Y=" }, StringSplitOptions.RemoveEmptyEntries);
            var buttonA = (int.Parse(parts[0]), int.Parse(parts[1]));
            var buttonB = (int.Parse(parts[2]), int.Parse(parts[3]));
            var prize = (int.Parse(parts[4]), int.Parse(parts[5]));

            return new ClawMachine(buttonA, buttonB, prize);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            long result = 0;

            List<ClawMachine> clawMachines = ParseInput(input);
            foreach (var clawMachine in clawMachines)
            {
                var (pressesA, pressesB) = clawMachine.GetFewestButtonPressesPartTwo();
                if (pressesA >= 0 && pressesB >= 0)
                {
                    result += pressesA * 3 + pressesB;
                }
            }

            InputOutputHelper.WriteOutput(isTest, result);
        }
    }

    class ClawMachine
    {
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (int x, int y) Prize { get; set; }

        public ClawMachine((int, int) buttonA, (int, int) buttonB, (int, int) prize)
        {
            ButtonA = buttonA;
            ButtonB = buttonB;
            Prize = prize;
        }
        
        public (long, long) GetFewestButtonPressesPartTwo()
        {
            double aX = ButtonA.x, aY = ButtonA.y;
            double bX = ButtonB.x, bY = ButtonB.y;
            double pX = Prize.x, pY = Prize.y;

            pX += 10000000000000;
            pY += 10000000000000;

            long j = (long)Math.Round((pY - (aY / aX) * pX) / (bY - (aY / aX) * bX));
            long i = (long)Math.Round((pX - bX * j) / aX);

            if (i * aX + j * bX == pX && i * aY + j * bY == pY)
            {
                return (i, j);
            }

            return (-1, -1);
        }

        public (int, int) GetFewestButtonPresses()
        {
            int aX = ButtonA.x, aY = ButtonA.y;
            int bX = ButtonB.x, bY = ButtonB.y;
            int pX = Prize.x, pY = Prize.y;

            for (int aPresses = 0; aPresses <= 100; aPresses++)
            {
                for (int bPresses = 0; bPresses <= 100; bPresses++)
                {
                    if (aPresses * aX + bPresses * bX == pX && aPresses * aY + bPresses * bY == pY)
                    {
                        return (aPresses, bPresses);
                    }
                }
            }

            return (-1, -1);
        }
    }
}