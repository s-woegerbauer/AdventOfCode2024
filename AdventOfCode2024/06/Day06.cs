using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day06
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "06");
            var input = InputOutputHelper.GetInput(false, "06");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var result = PredictGuardPath(input);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            int rows = input.Length;
            int cols = input[0].Length;
            int loopCount = 0;

            (int startRow, int startCol) = FindGuardStart(input);

            for (int obstacleRow = 0; obstacleRow < rows; obstacleRow++)
            {
                for (int obstacleCol = 0; obstacleCol < cols; obstacleCol++)
                {
                    int currentRow = startRow, currentCol = startCol;
                    int direction = 0;
                    var seenStates = new HashSet<(int, int, int)>();

                    while (true)
                    {
                        if (seenStates.Contains((currentRow, currentCol, direction)))
                        {
                            loopCount++;
                            break;
                        }
                        seenStates.Add((currentRow, currentCol, direction));
                        (int deltaRow, int deltaCol) = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) }[direction];
                        int newRow = currentRow + deltaRow;
                        int newCol = currentCol + deltaCol;

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                        {
                            break;
                        }
                        if (input[newRow][newCol] == '#' || (newRow == obstacleRow && newCol == obstacleCol))
                        {
                            direction = (direction + 1) % 4;
                        }
                        else
                        {
                            currentRow = newRow;
                            currentCol = newCol;
                        }
                    }
                }
            }

            InputOutputHelper.WriteOutput(isTest, loopCount);
        }

        private static (int, int) FindGuardStart(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == '^')
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        private static int PredictGuardPath(string[] input)
        {
            int rows = input.Length;
            int cols = input[0].Length;
            var visited = new HashSet<(int, int)>();
            var directions = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
            int dirIndex = 0;

            (int x, int y) = FindGuardStart(input);
            visited.Add((x, y));

            var queue = new Queue<(int, int, int)>();
            queue.Enqueue((x, y, dirIndex));

            while (queue.Count > 0)
            {
                var (currentX, currentY, currentDir) = queue.Dequeue();
                int newX = currentX + directions[currentDir].Item1;
                int newY = currentY + directions[currentDir].Item2;

                if (newX < 0 || newX >= rows || newY < 0 || newY >= cols || input[newX][newY] == '#')
                {
                    int newDir = (currentDir + 1) % 4;
                    queue.Enqueue((currentX, currentY, newDir));
                }
                else
                {
                    visited.Add((newX, newY));
                    queue.Enqueue((newX, newY, currentDir));
                }

                if (newX < 0 || newX >= rows || newY < 0 || newY >= cols)
                {
                    break;
                }
            }

            return visited.Count;
        }
    }
}