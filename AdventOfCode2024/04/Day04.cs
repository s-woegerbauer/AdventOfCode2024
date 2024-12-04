namespace AdventOfCode2024;

public static class Day04
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "04");
        var input = InputOutputHelper.GetInput(false, "04");
        
        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
        {
            var result = CountOccurrences(input);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var result = CountXMasOccurrences(input);

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static int CountOccurrences(string[] grid)
        {
            int count = 0;
            int rows = grid.Length;
            int cols = grid[0].Length;

            int[][] directions = new int[][]
            {
                new int[] { 0, 1 },
                new int[] { 1, 0 },
                new int[] { 1, 1 },   
                new int[] { 1, -1 }, 
                new int[] { 0, -1 }, 
                new int[] { -1, 0 }, 
                new int[] { -1, -1 }, 
                new int[] { -1, 1 }   
            };

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    foreach (var direction in directions)
                    {
                        if (IsWordPresent(grid, row, col, direction))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static bool IsWordPresent(string[] grid, int startRow, int startCol, int[] direction)
        {
            string word = "XMAS";
            int wordLength = word.Length;
            int rows = grid.Length;
            int cols = grid[0].Length;

            for (int i = 0; i < wordLength; i++)
            {
                int newRow = startRow + i * direction[0];
                int newCol = startCol + i * direction[1];

                if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || grid[newRow][newCol] != word[i])
                {
                    return false;
                }
            }

            return true;
        }
        
        
        private static int CountXMasOccurrences(string[] grid)
        {
            int count = 0;
            int rows = grid.Length;
            int cols = grid[0].Length;

            for (int row = 0; row < rows - 2; row++)
            {
                for (int col = 0; col < cols - 2; col++)
                {
                    if (IsXMasPresent(grid, row, col))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static bool IsXMasPresent(string[] grid, int startRow, int startCol)
        {
            string[] patterns = { "MAS", "SAM" };

            foreach (var pattern1 in patterns)
            {
                foreach (var pattern2 in patterns)
                {
                    if (grid[startRow][startCol] == pattern1[0] &&
                        grid[startRow + 1][startCol + 1] == pattern1[1] &&
                        grid[startRow + 2][startCol + 2] == pattern1[2] &&
                        grid[startRow][startCol + 2] == pattern2[0] &&
                        grid[startRow + 1][startCol + 1] == pattern2[1] &&
                        grid[startRow + 2][startCol] == pattern2[2])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
}