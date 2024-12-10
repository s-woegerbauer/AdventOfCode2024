using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day10
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "10");
            var input = InputOutputHelper.GetInput(false, "10");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var map = ParseInput(input);
            var trailheads = FindTrailheads(map);
            var totalScore = 0;

            foreach (var trailhead in trailheads)
            {
                var reachableNines = new HashSet<(int, int)>();
                FindTrails(map, trailhead.Item1, trailhead.Item2, 0, reachableNines);
                totalScore += reachableNines.Count;
            }

            InputOutputHelper.WriteOutput(isTest, totalScore);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var map = ParseInput(input);
            var trailheads = FindTrailheads(map);
            var totalScore = 0;

            foreach (var trailhead in trailheads)
            {
                int trails = 0;
                FindDistinctTrails(map, trailhead.Item1, trailhead.Item2, 0, ref trails);
                totalScore += trails;
            }

            InputOutputHelper.WriteOutput(isTest, totalScore);
        }

        private static int[,] ParseInput(string[] input)
        {
            var rows = input.Length;
            var cols = input[0].Length;
            var map = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    map[i, j] = input[i][j] - '0';
                }
            }

            return map;
        }

        private static List<(int, int)> FindTrailheads(int[,] map)
        {
            var trailheads = new List<(int, int)>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        trailheads.Add((i, j));
                    }
                }
            }
            return trailheads;
        }

        private static void FindTrails(int[,] map, int x, int y, int currentHeight, HashSet<(int, int)> reachableNines)
        {
            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1) || map[x, y] != currentHeight)
            {
                return;
            }

            if (currentHeight == 9)
            {
                reachableNines.Add((x, y));
                return;
            }

            var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var direction in directions)
            {
                FindTrails(map, x + direction.Item1, y + direction.Item2, currentHeight + 1, reachableNines);
            }
        }
        
        private static void FindDistinctTrails(int[,] map, int x, int y, int currentHeight, ref int trails)
        {
            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1) || map[x, y] != currentHeight)
            {
                return;
            }

            if (currentHeight == 9)
            {
                trails++;
                return;
            }

            var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var direction in directions)
            {
                FindDistinctTrails(map, x + direction.Item1, y + direction.Item2, currentHeight + 1, ref trails);
            }
        }
    }
}