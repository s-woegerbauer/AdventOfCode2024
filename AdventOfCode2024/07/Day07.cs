using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day07
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "07");
            var input = InputOutputHelper.GetInput(false, "07");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            long totalCalibrationResult = 0;

            foreach (var line in input)
            {
                var parts = line.Split(':');
                long testValue = long.Parse(parts[0].Trim());
                var numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();

                if (EvaluateAllCombinations(numbers, 0, numbers[0], testValue, false))
                {
                    totalCalibrationResult += testValue;
                }
            }

            InputOutputHelper.WriteOutput(isTest, totalCalibrationResult);
        }

        private static bool EvaluateAllCombinations(long[] numbers, int index, long currentValue, long testValue, bool isPartTwo)
        {
            if (index == numbers.Length - 1)
            {
                return currentValue == testValue;
            }

            int nextIndex = index + 1;
            long nextNumber = numbers[nextIndex];

            if (EvaluateAllCombinations(numbers, nextIndex, currentValue + nextNumber, testValue, isPartTwo))
            {
                return true;
            }

            if (EvaluateAllCombinations(numbers, nextIndex, currentValue * nextNumber, testValue, isPartTwo))
            {
                return true;
            }

            if (!isPartTwo)
            {
                return false;
            }
            
            long joinedValue = long.Parse(currentValue.ToString() + nextNumber.ToString());
            if (EvaluateAllCombinations(numbers, nextIndex, joinedValue, testValue, isPartTwo))
            {
                return true;
            }

            return false;
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            long totalCalibrationResult = 0;

            foreach (var line in input)
            {
                var parts = line.Split(':');
                long testValue = long.Parse(parts[0].Trim());
                var numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();

                if (EvaluateAllCombinations(numbers, 0, numbers[0], testValue, true))
                {
                    totalCalibrationResult += testValue;
                }
            }

            InputOutputHelper.WriteOutput(isTest, totalCalibrationResult);
        }
    }
}