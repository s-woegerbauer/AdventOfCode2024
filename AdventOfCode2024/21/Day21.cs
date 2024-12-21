using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2024;

public class Day21
{
    private static Dictionary<State, long> memo = new Dictionary<State, long>();

    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "21");
        var input = InputOutputHelper.GetInput(false, "21");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        long sum = 0;
        foreach (var s in input)
        {
            sum += CalculateResult(s) * int.Parse(s.TrimEnd('A'));
        }
        InputOutputHelper.WriteOutput(isTest, sum);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        long sum = 0;
        foreach (var s in input)
        {
            sum += CalculateResult(s, 26) * int.Parse(s.TrimEnd('A'));
        }
        InputOutputHelper.WriteOutput(isTest, sum);
    }

    private static long CalculateResult(string s, int numRobots = 3)
    {
        long result = 0;
        string padConfig = "789456123X0A";
        int currentRow = 3, currentCol = 2;

        foreach (char c in s)
        {
            var (nextRow, nextCol) = FindNextPosition(padConfig, c);
            result += Cheapest(new State(currentRow, currentCol, nextRow, nextCol, numRobots));
            currentRow = nextRow;
            currentCol = nextCol;
        }
        return result;
    }

    private static (int, int) FindNextPosition(string padConfig, char c)
    {
        for (int nextRow = 0; nextRow < 4; nextRow++)
        {
            for (int nextCol = 0; nextCol < 3; nextCol++)
            {
                if (padConfig[nextRow * 3 + nextCol] == c)
                {
                    return (nextRow, nextCol);
                }
            }
        }
        throw new ArgumentException("Invalid character in input");
    }

    private static long CheapestDirPad(State state)
    {
        if (memo.TryGetValue(state, out long cachedResult))
            return cachedResult;

        long answer = long.MaxValue;
        var queue = new Queue<(int row, int col, string presses)>();
        queue.Enqueue((state.CurrentRow, state.CurrentCol, ""));

        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            if (currentPosition.row == state.DestinationRow && currentPosition.col == state.DestinationCol)
            {
                long rec = CheapestRobot(currentPosition.presses + "A", state.NumRobots - 1);
                answer = Math.Min(answer, rec);
                continue;
            }
            if (currentPosition.row == 0 && currentPosition.col == 0)
                continue;

            EnqueueNextPositions(queue, currentPosition, state.DestinationRow, state.DestinationCol);
        }

        memo[state] = answer;
        return answer;
    }

    private static void EnqueueNextPositions(Queue<(int row, int col, string presses)> queue, (int row, int col, string presses) currentPosition, int destinationRow, int destinationCol)
    {
        if (currentPosition.row < destinationRow)
            queue.Enqueue((currentPosition.row + 1, currentPosition.col, currentPosition.presses + "v"));
        else if (currentPosition.row > destinationRow)
            queue.Enqueue((currentPosition.row - 1, currentPosition.col, currentPosition.presses + "^"));

        if (currentPosition.col < destinationCol)
            queue.Enqueue((currentPosition.row, currentPosition.col + 1, currentPosition.presses + ">"));
        else if (currentPosition.col > destinationCol)
            queue.Enqueue((currentPosition.row, currentPosition.col - 1, currentPosition.presses + "<"));
    }

    private static long CheapestRobot(string presses, int numRobots)
    {
        if (numRobots == 1)
            return presses.Length;

        long result = 0;
        string padConfig = "X^A<v>";

        int currentRow = 0, currentCol = 2;

        foreach (char press in presses)
        {
            var (nextRow, nextCol) = FindNextPosition(padConfig, press);
            result += CheapestDirPad(new State(currentRow, currentCol, nextRow, nextCol, numRobots));
            currentRow = nextRow;
            currentCol = nextCol;
        }
        return result;
    }

    private static long Cheapest(State state)
    {
        long answer = long.MaxValue;
        var queue = new Queue<(int row, int col, string presses)>();
        queue.Enqueue((state.CurrentRow, state.CurrentCol, ""));

        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            if (currentPosition.row == state.DestinationRow && currentPosition.col == state.DestinationCol)
            {
                long rec = CheapestRobot(currentPosition.presses + "A", state.NumRobots);
                answer = Math.Min(answer, rec);
                continue;
            }
            if (currentPosition.row == 3 && currentPosition.col == 0)
                continue;

            EnqueueNextPositions(queue, currentPosition, state.DestinationRow, state.DestinationCol);
        }
        return answer;
    }

    private struct State
    {
        public int CurrentRow { get; }
        public int CurrentCol { get; }
        public int DestinationRow { get; }
        public int DestinationCol { get; }
        public int NumRobots { get; }

        public State(int currentRow, int currentCol, int destinationRow, int destinationCol, int numRobots)
        {
            CurrentRow = currentRow;
            CurrentCol = currentCol;
            DestinationRow = destinationRow;
            DestinationCol = destinationCol;
            NumRobots = numRobots;
        }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                return CurrentRow == other.CurrentRow &&
                       CurrentCol == other.CurrentCol &&
                       DestinationRow == other.DestinationRow &&
                       DestinationCol == other.DestinationCol &&
                       NumRobots == other.NumRobots;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrentRow, CurrentCol, DestinationRow, DestinationCol, NumRobots);
        }
    }
}