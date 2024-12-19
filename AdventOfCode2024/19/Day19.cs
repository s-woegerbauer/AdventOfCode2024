using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day19
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "19");
            var input = InputOutputHelper.GetInput(false, "19");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var (patterns, designs) = ParseInput(input);
            var result = CountPossibleDesigns(patterns, designs);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var (patterns, designs) = ParseInput(input);
            var result = CountAllPossibleWays(patterns, designs);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static (HashSet<string> patterns, List<string> designs) ParseInput(string[] input)
        {
            var patterns = new HashSet<string>(input[0].Split(", "));
            var designs = input.Skip(2).ToList();
            return (patterns, designs);
        }

        private static int CountPossibleDesigns(HashSet<string> patterns, List<string> designs)
        {
            int possibleCount = 0;

            foreach (var design in designs)
            {
                if (IsDesignPossible(patterns, design))
                {
                    possibleCount++;
                }
            }

            return possibleCount;
        }

        private static bool IsDesignPossible(HashSet<string> patterns, string design)
        {
            int n = design.Length;
            bool[] dp = new bool[n + 1];
            dp[0] = true;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (dp[j] && patterns.Contains(design.Substring(j, i - j)))
                    {
                        dp[i] = true;
                        break;
                    }
                }
            }

            return dp[n];
        }

        private static long CountAllPossibleWays(HashSet<string> patterns, List<string> designs)
        {
            long totalWays = 0;

            foreach (var design in designs)
            {
                totalWays += CountWays(patterns, design);
            }

            return totalWays;
        }

        private static long CountWays(HashSet<string> patterns, string design)
        {
            int n = design.Length;
            long[] dp = new long[n + 1];
            dp[0] = 1;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (patterns.Contains(design.Substring(j, i - j)))
                    {
                        dp[i] += dp[j];
                    }
                }
            }

            return dp[n];
        }
    }
}