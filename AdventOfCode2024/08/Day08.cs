using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day08
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "08");
            var input = InputOutputHelper.GetInput(false, "08");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var city = ParseInput(input);
            var antennas = GetAntennas(city);
            var antinodes = GetAntinodes(city, antennas);

            InputOutputHelper.WriteOutput(isTest, antinodes.Count);
        }

        private static char[,] ParseInput(string[] input)
        {
            char[,] city = new char[input.Length, input[0].Length];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    city[i, j] = input[i][j];
                }
            }

            return city;
        }

        private static List<(int x, int y, char freq)> GetAntennas(char[,] city)
        {
            var antennas = new List<(int x, int y, char freq)>();
            for (int i = 0; i < city.GetLength(0); i++)
            {
                for (int j = 0; j < city.GetLength(1); j++)
                {
                    if (city[i, j] != '.')
                    {
                        antennas.Add((i, j, city[i, j]));
                    }
                }
            }

            return antennas;
        }

        private static HashSet<(int x, int y)> GetAntinodes(char[,] city, List<(int x, int y, char freq)> antennas)
        {
            var antinodes = new HashSet<(int x, int y)>();
            for (int i = 0; i < antennas.Count; i++)
            {
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    if (antennas[i].freq == antennas[j].freq)
                    {
                        var (x1, y1, _) = antennas[i];
                        var (x2, y2, _) = antennas[j];
                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        int ax1 = x1 - dx;
                        int ay1 = y1 - dy;
                        int ax2 = x2 + dx;
                        int ay2 = y2 + dy;

                        if (IsValid(city, ax1, ay1))
                        {
                            antinodes.Add((ax1, ay1));
                        }

                        if (IsValid(city, ax2, ay2))
                        {
                            antinodes.Add((ax2, ay2));
                        }
                    }
                }
            }

            return antinodes;
        }

        private static bool IsValid(char[,] city, int x, int y)
        {
            return x >= 0 && x < city.GetLength(0) && y >= 0 && y < city.GetLength(1);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var city = ParseInput(input);
            var antennas = GetAntennas(city);
            var antinodes = GetAntinodesWithHarmonics(city, antennas);

            PrintCityWithAntinodes(city, antinodes, antennas);
            InputOutputHelper.WriteOutput(isTest, antinodes.Count);
        }

        private static void PrintCityWithAntinodes(char[,] city, HashSet<(int x, int y)> antinodes,
            List<(int x, int y, char freq)> antennas)
        {
            for (int i = 0; i < city.GetLength(0); i++)
            {
                for (int j = 0; j < city.GetLength(1); j++)
                {
                    if (antennas.Contains((i, j, city[i, j])))
                    {
                        Console.Write(city[i, j].ToString());
                    }
                    else if (antinodes.Contains((i, j)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }
        }

        private static HashSet<(int x, int y)> GetAntinodesWithHarmonics(char[,] city,
            List<(int x, int y, char freq)> antennas)
        {
            var antinodes = new HashSet<(int x, int y)>();
            var frequencyGroups = new Dictionary<char, List<(int x, int y)>>();

            foreach (var antenna in antennas)
            {
                if (!frequencyGroups.ContainsKey(antenna.freq))
                {
                    frequencyGroups[antenna.freq] = new List<(int x, int y)>();
                }

                frequencyGroups[antenna.freq].Add((antenna.x, antenna.y));
            }

            foreach (var group in frequencyGroups.Values)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    for (int j = i + 1; j < group.Count; j++)
                    {
                        var (x1, y1) = group[i];
                        var (x2, y2) = group[j];
                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        antinodes.Add((x1, y1));
                        antinodes.Add((x2, y2));

                        int k = 1;
                        while (true)
                        {
                            int ax1 = x1 - k * dx;
                            int ay1 = y1 - k * dy;
                            int ax2 = x2 + k * dx;
                            int ay2 = y2 + k * dy;

                            bool added = false;
                            if (IsValid(city, ax1, ay1))
                            {
                                antinodes.Add((ax1, ay1));
                                added = true;
                            }

                            if (IsValid(city, ax2, ay2))
                            {
                                antinodes.Add((ax2, ay2));
                                added = true;
                            }

                            if (!added)
                            {
                                break;
                            }

                            k++;
                        }
                    }
                }
            }

            return antinodes;
        }
    }
}