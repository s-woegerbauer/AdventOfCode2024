using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day12
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "12");
            var input = InputOutputHelper.GetInput(false, "12");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var garden = ParseInput(input);
            var result = CalculateTotalPrice(garden);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var garden = ParseInput(input);
            var result = CalculateTotalScore(garden);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static char[,] ParseInput(string[] input)
        {
            int rows = input.Length;
            int cols = input[0].Length;
            var result = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = input[i][j];
                }
            }

            return result;
        }

        private static int CalculateTotalPrice(char[,] garden)
        {
            int rows = garden.GetLength(0);
            int cols = garden.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            int totalPrice = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!visited[i, j])
                    {
                        char plantType = garden[i, j];
                        (int area, int perimeter) = FloodFill(garden, visited, i, j, plantType);
                        int price = area * perimeter;
                        totalPrice += price;
                    }
                }
            }

            return totalPrice;
        }

        private static int CalculateTotalScore(char[,] garden)
        {
            int rows = garden.GetLength(0);
            int cols = garden.GetLength(1);
            var seen = new HashSet<(int, int)>();
            int total = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    char c = Get(garden, x, y);
                    var (a, p) = Score(garden, x, y, c, seen);
                    total += a * p;
                }
            }

            return total;
        }

        private static char Get(char[,] garden, int x, int y)
        {
            int rows = garden.GetLength(0);
            int cols = garden.GetLength(1);
            if (x < 0 || y < 0 || x >= cols || y >= rows)
            {
                return '.';
            }
            return garden[y, x];
        }

        private static (int, int) Score(char[,] garden, int x, int y, char c, HashSet<(int, int)> seen)
        {
            if (seen.Contains((x, y)) || Get(garden, x, y) != c)
            {
                return (0, 0);
            }
            seen.Add((x, y));
            int area = 1, perimeter = 0;
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                if (Get(garden, nx, ny) != c)
                {
                    perimeter++;
                    if (dx[i] == 0 && dy[i] == 1 && Get(garden, x + 1, y) == c && Get(garden, x + 1, y + 1) != c)
                    {
                        perimeter--;
                    }
                    if (dx[i] == 0 && dy[i] == -1 && Get(garden, x + 1, y) == c && Get(garden, x + 1, y - 1) != c)
                    {
                        perimeter--;
                    }
                    if (dx[i] == 1 && dy[i] == 0 && Get(garden, x, y + 1) == c && Get(garden, x + 1, y + 1) != c)
                    {
                        perimeter--;
                    }
                    if (dx[i] == -1 && dy[i] == 0 && Get(garden, x, y + 1) == c && Get(garden, x - 1, y + 1) != c)
                    {
                        perimeter--;
                    }
                }
                else
                {
                    var result = Score(garden, nx, ny, c, seen);
                    area += result.Item1;
                    perimeter += result.Item2;
                }
            }

            return (area, perimeter);
        }

        private static (int, int) FloodFill(char[,] garden, bool[,] visited, int x, int y, char plantType)
        {
            int rows = garden.GetLength(0);
            int cols = garden.GetLength(1);
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((x, y));
            visited[x, y] = true;

            int area = 0;
            int perimeter = 0;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Pop();
                area++;

                for (int i = 0; i < 4; i++)
                {
                    int nx = cx + dx[i];
                    int ny = cy + dy[i];

                    if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                    {
                        if (garden[nx, ny] == plantType && !visited[nx, ny])
                        {
                            stack.Push((nx, ny));
                            visited[nx, ny] = true;
                        }
                        else if (garden[nx, ny] != plantType)
                        {
                            perimeter++;
                        }
                    }
                    else
                    {
                        perimeter++;
                    }
                }
            }

            return (area, perimeter);
        }
    }
}