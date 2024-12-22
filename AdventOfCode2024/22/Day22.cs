using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day22
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "22");
            var input = InputOutputHelper.GetInput(false, "22");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var result = CalculateSumOf2000thSecretNumbers(input);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static long CalculateSumOf2000thSecretNumbers(string[] input)
        {
            return input.Select(long.Parse)
                .Select(Generate2000thSecretNumber)
                .Sum();
        }

        private static long Generate2000thSecretNumber(long secret)
        {
            for (int i = 0; i < 2000; i++)
            {
                secret = EvolveSecretNumber(secret);
            }
            return secret;
        }

        private static long EvolveSecretNumber(long secret)
        {
            secret = MixAndPrune(secret, secret * 64);
            secret = MixAndPrune(secret, secret / 32);
            secret = MixAndPrune(secret, secret * 2048);
            return secret;
        }

        private static long MixAndPrune(long secret, long value)
        {
            secret ^= value;
            secret %= 16777216;
            return secret;
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var sequences = new Dictionary<(int, int, int, int), int>();

            foreach (var numStr in input)
            {
                long number = long.Parse(numStr);
                ProcessNumber(number, sequences);
            }

            InputOutputHelper.WriteOutput(isTest, sequences.Values.Max());
        }

        private static void ProcessNumber(long number, Dictionary<(int, int, int, int), int> sequences)
        {
            var sequenceSet = new HashSet<(int, int, int, int)>();
            var window = new int?[] { null, null, null, null };
            int previousDigit = (int)(number % 10);

            for (int i = 0; i < 2000; i++)
            {
                number = EvolveSecretNumber(number);
                int currentDigit = (int)(number % 10);
                int difference = currentDigit - previousDigit;
                previousDigit = currentDigit;

                UpdateWindow(window, difference);

                if (window.Contains(null))
                {
                    continue;
                }

                var sequence = (window[0].Value, window[1].Value, window[2].Value, window[3].Value);

                if (sequenceSet.Add(sequence))
                {
                    if (!sequences.ContainsKey(sequence))
                    {
                        sequences[sequence] = currentDigit;
                    }
                    else
                    {
                        sequences[sequence] += currentDigit;
                    }
                }
            }
        }

        private static void UpdateWindow(int?[] window, int difference)
        {
            for (int j = 0; j < window.Length - 1; j++)
            {
                window[j] = window[j + 1];
            }
            window[window.Length - 1] = difference;
        }
    }
}