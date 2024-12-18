namespace AdventOfCode2024;

public static class Day18
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "18");
        var input = InputOutputHelper.GetInput(false, "18");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var byteCount = isTest ? 12 : 1024;
        var boolArray = ParseInput(input, isTest, byteCount);
        var result = FindShortestPath(boolArray);
        PrintGrid(boolArray);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PrintGrid(bool[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j] ? "#" : ".");
            }
            Console.WriteLine();
        }
    }

    private static int FindShortestPath(bool[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var queue = new Queue<(int, int, int)>();
        var visited = new bool[rows, cols];

        queue.Enqueue((0, 0, 0));
        visited[0, 0] = true;

        while (queue.Count > 0)
        {
            var (x, y, dist) = queue.Dequeue();

            if (x == rows - 1 && y == cols - 1)
            {
                return dist;
            }

            foreach (var (dx, dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !grid[newX, newY] && !visited[newX, newY])
                {
                    queue.Enqueue((newX, newY, dist + 1));
                    visited[newX, newY] = true;
                }
            }
        }

        return -1;
    }

    private static bool[,] ParseInput(string[] input, bool isTest, int byteCount)
    {
        var coordinates = input.Select(line => line.Split(',').Select(int.Parse).ToArray()).ToArray();
        var sideMax = isTest ? 7 : 71;

        var boolArray = new bool[sideMax, sideMax];

        for (int i = 0; i < Math.Min(byteCount, coordinates.Length); i++)
        {
            int x = coordinates[i][0];
            int y = coordinates[i][1];
            boolArray[y, x] = true;
        }

        return boolArray;
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var byteCount = isTest ? 12 : 1024;
        var boolArray = ParseInput(input, isTest, byteCount);
        var coordinates = input.Select(line => line.Split(',').Select(int.Parse).ToArray()).ToArray();

        for (int i = 0; i < coordinates.Length; i++)
        {
            int x = coordinates[i][0];
            int y = coordinates[i][1];
            boolArray[y, x] = true;

            var result = FindShortestPath(boolArray);
            if (result == -1)
            {
                InputOutputHelper.WriteOutput(isTest, $"{x},{y}");
                return;
            }
        }
    }
}