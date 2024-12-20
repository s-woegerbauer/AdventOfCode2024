using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public static class Day20
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "20");
            var input = InputOutputHelper.GetInput(false, "20");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var matrix = ParseInput(input);
            var result = isTest ? Calculate(matrix, 2, 2) : Calculate(matrix, 2);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var matrix = ParseInput(input);
            var result = isTest ? Calculate(matrix, 20, 50) : Calculate(matrix, 20);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static Matrix2D<char> ParseInput(string[] input)
        {
            int rowCount = input.Length;
            int colCount = input[0].Length;
            var matrix = new Matrix2D<char>(rowCount, colCount);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    matrix[row, col] = input[row][col];
                }
            }

            return matrix;
        }

        private static ulong Calculate(Matrix2D<char> matrix, int range, int minDistance = 100)
        {
            var endPosition = new Vector2();
            var startPosition = new Vector2();
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColCount; col++)
                {
                    if (matrix[row, col] == 'E')
                    {
                        endPosition = new Vector2(col, row);
                    }

                    if (matrix[row, col] == 'S')
                    {
                        startPosition = new Vector2(col, row);
                    }
                }
            }

            var endDistances = new Matrix2D<ulong>(matrix.RowCount, matrix.ColCount);
            FillMatrix(endDistances, ulong.MaxValue);

            var startDistances = new Matrix2D<ulong>(matrix.RowCount, matrix.ColCount);
            FillMatrix(startDistances, ulong.MaxValue);

            CalculateDistances(matrix, endPosition, endDistances);
            CalculateDistances(matrix, startPosition, startDistances);

            ulong validPathsCount = 0;
            ulong originalDistance = endDistances[startPosition.Y, startPosition.X];
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColCount; col++)
                {
                    var midPoint = new Vector2(col, row);
                    if (matrix[row, col] == '#' || startDistances[row, col] == ulong.MaxValue)
                    {
                        continue;
                    }

                    for (int rowOffset = -range; rowOffset <= range; rowOffset++)
                    {
                        for (int colOffset = -range; colOffset <= range; colOffset++)
                        {
                            var offsetVector = new Vector2(colOffset, rowOffset);
                            if (offsetVector.ManhattanDistance(new Vector2()) > range)
                            {
                                continue;
                            }

                            var endPoint = midPoint + offsetVector;
                            if (!matrix.Contains(endPoint) || matrix[endPoint.Y, endPoint.X] == '#' || endDistances[endPoint.Y, endPoint.X] == ulong.MaxValue)
                            {
                                continue;
                            }

                            ulong newDistance = startDistances[midPoint.Y, midPoint.X] + endDistances[endPoint.Y, endPoint.X] + (ulong)offsetVector.ManhattanDistance(new Vector2());
                            if (newDistance + (ulong)minDistance <= originalDistance)
                            {
                                validPathsCount++;
                            }
                        }
                    }
                }
            }

            return validPathsCount;
        }

        private static void FillMatrix(Matrix2D<ulong> matrix, ulong value)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColCount; col++)
                {
                    matrix[row, col] = value;
                }
            }
        }

        private static void CalculateDistances(Matrix2D<char> matrix, Vector2 startPosition, Matrix2D<ulong> distances)
        {
            ulong currentCost = 0;
            var currentQueue = new Queue<Vector2>();
            currentQueue.Enqueue(startPosition);
            var nextQueue = new Queue<Vector2>();

            while (currentQueue.Count > 0)
            {
                while (currentQueue.Count > 0)
                {
                    var position = currentQueue.Dequeue();
                    if (distances[position.Y, position.X] != ulong.MaxValue)
                    {
                        continue;
                    }

                    distances[position.Y, position.X] = currentCost;
                    foreach (var adjacent in position.AdjacentPoints())
                    {
                        if (!matrix.Contains(adjacent) || distances[adjacent.Y, adjacent.X] != ulong.MaxValue || matrix[adjacent.Y, adjacent.X] == '#')
                        {
                            continue;
                        }

                        nextQueue.Enqueue(adjacent);
                    }
                }

                var tempQueue = currentQueue;
                currentQueue = nextQueue;
                nextQueue = tempQueue;
                currentCost++;
            }
        }
    }

    public class Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

        public IEnumerable<Vector2> AdjacentPoints()
        {
            yield return new Vector2(X + 1, Y);
            yield return new Vector2(X - 1, Y);
            yield return new Vector2(X, Y + 1);
            yield return new Vector2(X, Y - 1);
        }

        public int ManhattanDistance(Vector2 other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public class Matrix2D<T>
    {
        private readonly T[,] _data;

        public Matrix2D(int rows, int cols)
        {
            _data = new T[rows, cols];
        }

        public T this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public int RowCount => _data.GetLength(0);
        public int ColCount => _data.GetLength(1);

        public bool Contains(Vector2 pos) => pos.X >= 0 && pos.X < ColCount && pos.Y >= 0 && pos.Y < RowCount;
    }
}