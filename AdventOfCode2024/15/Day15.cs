using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2024;

public static class Day15
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "15");
        var input = InputOutputHelper.GetInput(false, "15");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var (grid, instructions) = ParseInput(input);
        var robot = grid.OfType<Robot>().First();
        robot.Move(instructions);
        PrintGrid(grid);
        var result = CalculateResult(grid);
        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var (initialGrid, instructions) = ParseInputPartTwo(input);
        var expandedGrid = ExpandGrid(initialGrid);
        var (robotRow, robotCol) = FindRobot(expandedGrid);

        foreach (var instruction in instructions)
        {
            (robotRow, robotCol) = ProcessInstruction(expandedGrid, robotRow, robotCol, instruction);
        }

        InputOutputHelper.WriteOutput(isTest, CalculateGPS(expandedGrid));
    }

    private static void PrintGrid(Entity[,] grid)
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                Console.Write(grid[row, col] switch
                {
                    Wall _ => '#',
                    Box _ => 'O',
                    Robot _ => '@',
                    _ => '.'
                });
            }
            Console.WriteLine();
        }
    }

    private static (Entity[,], List<char>) ParseInput(string[] input)
    {
        var gridLines = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        var instructionLines = input.Skip(gridLines.Length + 1).ToArray();

        int rows = gridLines.Length;
        int cols = gridLines[0].Length;
        Entity[,] grid = new Entity[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                grid[row, col] = gridLines[row][col] switch
                {
                    '#' => new Wall(new Point(col, row), grid),
                    'O' => new Box(new Point(col, row), grid),
                    '@' => new Robot(new Point(col, row), grid),
                    _ => new Void(new Point(col, row), grid)
                };
            }
        }

        var instructions = new List<char>();
        foreach (var line in instructionLines)
        {
            instructions.AddRange(line);
        }

        return (grid, instructions);
    }

    private static int CalculateResult(Entity[,] grid)
    {
        int result = 0;
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] is Box)
                {
                    result += 100 * row + col;
                }
            }
        }
        return result;
    }

    private static (List<char[]>, string) ParseInputPartTwo(string[] input)
    {
        var gridLines = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        var instructionLines = input.Skip(gridLines.Length + 1).ToArray();

        int rows = gridLines.Length;
        List<char[]> grid = new List<char[]>();
        for (int row = 0; row < rows; row++)
        {
            grid.Add(gridLines[row].ToCharArray());
        }

        var instructions = string.Concat(instructionLines);
        return (grid, instructions);
    }

    private static char[][] ExpandGrid(List<char[]> initialGrid)
    {
        var expandedGrid = new List<char[]>();
        for (int row = 0; row < initialGrid.Count; row++)
        {
            var newRow = new List<char>();
            for (int col = 0; col < initialGrid[row].Length; col++)
            {
                switch (initialGrid[row][col])
                {
                    case '#':
                        newRow.Add('#');
                        newRow.Add('#');
                        break;
                    case 'O':
                        newRow.Add('[');
                        newRow.Add(']');
                        break;
                    case '.':
                        newRow.Add('.');
                        newRow.Add('.');
                        break;
                    case '@':
                        newRow.Add('@');
                        newRow.Add('.');
                        break;
                }
            }
            expandedGrid.Add(newRow.ToArray());
        }
        return expandedGrid.ToArray();
    }

    private static (int row, int col) FindRobot(char[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '@')
                    return (row, col);
            }
        }
        return (-1, -1);
    }

    private static IEnumerable<(int row, int col)> FindBoxes(char[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == 'O' || grid[row][col] == '[')
                    yield return (row, col);
            }
        }
    }

    private static bool IsValidMove(char[][] grid, int row, int col)
    {
        return row >= 0 && row < grid.Length && col >= 0 && col < grid[row].Length && grid[row][col] != '#';
    }

    private static bool CanMove(char[][] grid, int row, int col, int rowDelta, int colDelta, HashSet<(int, int)> visited)
    {
        if (visited.Contains((row, col)))
            return true;

        visited.Add((row, col));

        int newRow = row + rowDelta;
        int newCol = col + colDelta;

        switch (grid[newRow][newCol])
        {
            case '#':
                return false;
            case '[':
                return CanMove(grid, newRow, newCol, rowDelta, colDelta, visited) && CanMove(grid, newRow, newCol + 1, rowDelta, colDelta, visited);
            case ']':
                return CanMove(grid, newRow, newCol, rowDelta, colDelta, visited) && CanMove(grid, newRow, newCol - 1, rowDelta, colDelta, visited);
            case 'O':
                return CanMove(grid, newRow, newCol, rowDelta, colDelta, visited);
        }
        return true;
    }

    private static (int row, int col) ProcessInstruction(char[][] grid, int row, int col, char instruction)
    {
        var (rowDelta, colDelta) = DIRECTIONS[instruction];

        int newRow = row + rowDelta;
        int newCol = col + colDelta;

        if (!IsValidMove(grid, newRow, newCol))
            return (row, col);

        if (grid[newRow][newCol] == '[' || grid[newRow][newCol] == ']' || grid[newRow][newCol] == 'O')
        {
            var visited = new HashSet<(int, int)>();

            if (!CanMove(grid, row, col, rowDelta, colDelta, visited))
                return (row, col);

            while (visited.Count > 0)
            {
                foreach (var (r, c) in visited.ToArray())
                {
                    int nextRow = r + rowDelta;
                    int nextCol = c + colDelta;

                    if (!visited.Contains((nextRow, nextCol)))
                    {
                        if (grid[nextRow][nextCol] != '@' && grid[r][c] != '@')
                        {
                            grid[nextRow][nextCol] = grid[r][c];
                            grid[r][c] = '.';
                        }

                        visited.Remove((r, c));
                    }
                }
            }

            (grid[row][col], grid[newRow][newCol]) = (grid[newRow][newCol], grid[row][col]);
            return (newRow, newCol);
        }

        (grid[row][col], grid[newRow][newCol]) = (grid[newRow][newCol], grid[row][col]);
        return (newRow, newCol);
    }

    private static int CalculateGPS(char[][] grid)
    {
        return FindBoxes(grid).Sum(box => 100 * box.row + box.col);
    }

    static readonly Dictionary<char, (int rowDelta, int colDelta)> DIRECTIONS = new()
    {
        { '<', (0, -1) },
        { '>', (0, 1) },
        { '^', (-1, 0) },
        { 'v', (1, 0) }
    };

    public class Entity
    {
        public Point Position { get; set; }
        public Entity[,] Grid { get; set; }

        public Entity(Point position, Entity[,] grid)
        {
            Position = position;
            Grid = grid;
        }

        public virtual bool GetPushed(Point direction)
        {
            var newPosition = new Point(Position.X + direction.X, Position.Y + direction.Y);

            if (Grid[newPosition.Y, newPosition.X] is Void)
            {
                Grid[Position.Y, Position.X] = new Void(Position, Grid);
                Grid[newPosition.Y, newPosition.X] = this;
                Position = newPosition;
                return true;
            }

            if (Grid[newPosition.Y, newPosition.X] is Wall)
            {
                return false;
            }

            if (Grid[newPosition.Y, newPosition.X] is Box)
            {
                if (!Grid[newPosition.Y, newPosition.X].GetPushed(direction))
                {
                    return false;
                }
            }

            Grid[Position.Y, Position.X] = new Void(Position, Grid);
            Grid[newPosition.Y, newPosition.X] = this;
            Position = newPosition;
            return true;
        }
    }

    public class Robot : Entity
    {
        public Robot(Point position, Entity[,] grid) : base(position, grid) { }

        public void Move(List<char> instructions)
        {
            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case '^':
                        GetPushed(new Point(0, -1));
                        break;
                    case 'v':
                        GetPushed(new Point(0, 1));
                        break;
                    case '<':
                        GetPushed(new Point(-1, 0));
                        break;
                    case '>':
                        GetPushed(new Point(1, 0));
                        break;
                }
            }
        }
    }

    public class Wall : Entity
    {
        public Wall(Point position, Entity[,] grid) : base(position, grid) { }
    }

    public class Box : Entity
    {
        public Box(Point position, Entity[,] grid) : base(position, grid) { }
    }

    public class Void : Entity
    {
        public Void(Point position, Entity[,] grid) : base(position, grid) { }
    }
}