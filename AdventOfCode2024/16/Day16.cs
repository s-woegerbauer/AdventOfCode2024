using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024;

public static class Day16
{
    private static readonly (int dx, int dy)[]
        Directions = { (1, 0), (0, 1), (-1, 0), (0, -1) };

    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "16");
        var input = InputOutputHelper.GetInput(false, "16");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var grid = new char[input.Length, input[0].Length];
        var start = (x: 0, y: 0);
        var end = (x: 0, y: 0);

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                grid[y, x] = input[y][x];
                if (grid[y, x] == 'S') start = (x, y);
                if (grid[y, x] == 'E') end = (x, y);
            }
        }

        var visited = Dijkstra(grid, start, end, input.Length, input[0].Length);

        var result = Enumerable.Range(0, 4)
            .Where(d => visited.ContainsKey((end.x, end.y, d)))
            .Min(d => visited[(end.x, end.y, d)]);
        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var grid = new char[input.Length, input[0].Length];
        var start = (x: 0, y: 0);
        var end = (x: 0, y: 0);

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                grid[y, x] = input[y][x];
                if (grid[y, x] == 'S') start = (x, y);
                if (grid[y, x] == 'E') end = (x, y);
            }
        }

        var visited = Dijkstra(grid, start, end, input.Length, input[0].Length);
        var bestPathTiles = BacktrackShortestPaths(grid, visited, end, input.Length, input[0].Length);

        var result = bestPathTiles.Count;
        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static Dictionary<(int x, int y, int d), int> Dijkstra(char[,] grid, (int x, int y) start,
        (int x, int y) end, int rows, int cols)
    {
        var directions = new (int dx, int dy)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
        var startState = (start.x, start.y, 1);

        var pq = new PriorityQueue<(int cost, (int x, int y, int d) state), int>();
        pq.Enqueue((0, startState), 0);
        var visited = new Dictionary<(int x, int y, int d), int> { [startState] = 0 };

        while (pq.Count > 0)
        {
            var (cost, (x, y, d)) = pq.Dequeue();

            if (visited.GetValueOrDefault((x, y, d), int.MaxValue) < cost)
                continue;

            var (dx, dy) = directions[d];
            var nx = x + dx;
            var ny = y + dy;
            if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && grid[ny, nx] != '#')
            {
                var newCost = cost + 1;
                if (newCost < visited.GetValueOrDefault((nx, ny, d), int.MaxValue))
                {
                    visited[(nx, ny, d)] = newCost;
                    pq.Enqueue((newCost, (nx, ny, d)), newCost);
                }
            }

            foreach (var nd in new[] { (d - 1 + 4) % 4, (d + 1) % 4 })
            {
                var newCost = cost + 1000;
                if (newCost < visited.GetValueOrDefault((x, y, nd), int.MaxValue))
                {
                    visited[(x, y, nd)] = newCost;
                    pq.Enqueue((newCost, (x, y, nd)), newCost);
                }
            }
        }

        return visited;
    }

    private static HashSet<(int x, int y)> BacktrackShortestPaths(char[,] grid,
        Dictionary<(int x, int y, int d), int> visited, (int x, int y) end, int rows, int cols)
    {
        var directions = new (int dx, int dy)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
        var minEndCost = Enumerable.Range(0, 4).Where(d => visited.ContainsKey((end.x, end.y, d)))
            .Min(d => visited[(end.x, end.y, d)]);

        var onShortestPath = new HashSet<(int x, int y, int d)>();
        var q = new Queue<(int x, int y, int d)>();
        foreach (var d in Enumerable.Range(0, 4).Where(d =>
                     visited.ContainsKey((end.x, end.y, d)) && visited[(end.x, end.y, d)] == minEndCost))
        {
            var edState = (end.x, end.y, d);
            onShortestPath.Add(edState);
            q.Enqueue(edState);
        }

        while (q.Count > 0)
        {
            var (cx, cy, cd) = q.Dequeue();
            var currentCost = visited[(cx, cy, cd)];

            var (dx, dy) = directions[cd];
            var px = cx - dx;
            var py = cy - dy;
            if (px >= 0 && px < rows && py >= 0 && py < cols && grid[py, px] != '#')
            {
                var prevCost = currentCost - 1;
                if (prevCost >= 0)
                {
                    var prevState = (px, py, cd);
                    if (visited.GetValueOrDefault(prevState, int.MaxValue) == prevCost &&
                        onShortestPath.Add(prevState))
                    {
                        q.Enqueue(prevState);
                    }
                }
            }

            var turnCost = currentCost - 1000;
            if (turnCost >= 0)
            {
                foreach (var pd in new[] { (cd - 1 + 4) % 4, (cd + 1) % 4 })
                {
                    var prevState = (cx, cy, pd);
                    if (visited.GetValueOrDefault(prevState, int.MaxValue) == turnCost &&
                        onShortestPath.Add(prevState))
                    {
                        q.Enqueue(prevState);
                    }
                }
            }
        }

        return new HashSet<(int x, int y)>(onShortestPath.Select(state => (state.x, state.y)));
    }
}