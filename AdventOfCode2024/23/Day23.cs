using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day23
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "23");
            var input = InputOutputHelper.GetInput(false, "23");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var graph = BuildGraph(input);
            var triangles = FindTriangles(graph);
            var result = triangles.Count(triangle => triangle.Any(node => node.StartsWith("t")));
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var graph = BuildGraph(input);
            var largestClique = FindLargestGroup(graph);
            var password = string.Join(",", largestClique.OrderBy(node => node));
            InputOutputHelper.WriteOutput(isTest, password);
        }

        private static Dictionary<string, HashSet<string>> BuildGraph(string[] input)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var nodes = line.Split('-');
                if (!graph.ContainsKey(nodes[0]))
                {
                    graph[nodes[0]] = new HashSet<string>();
                }
                if (!graph.ContainsKey(nodes[1]))
                {
                    graph[nodes[1]] = new HashSet<string>();
                }
                graph[nodes[0]].Add(nodes[1]);
                graph[nodes[1]].Add(nodes[0]);
            }
            return graph;
        }

        private static List<HashSet<string>> FindTriangles(Dictionary<string, HashSet<string>> graph)
        {
            var triangles = new List<HashSet<string>>();
            foreach (var node in graph.Keys)
            {
                foreach (var neighbor in graph[node])
                {
                    foreach (var neighborOfNeighbor in graph[neighbor])
                    {
                        if (neighborOfNeighbor != node && graph[neighborOfNeighbor].Contains(node))
                        {
                            var triangle = new HashSet<string> { node, neighbor, neighborOfNeighbor };
                            if (!triangles.Any(t => t.SetEquals(triangle)))
                            {
                                triangles.Add(triangle);
                            }
                        }
                    }
                }
            }
            return triangles;
        }

        private static HashSet<string> FindLargestGroup(Dictionary<string, HashSet<string>> graph)
        {
            var largestClique = new HashSet<string>();
            foreach (var node in graph.Keys)
            {
                var clique = new HashSet<string> { node };
                foreach (var neighbor in graph[node])
                {
                    if (graph[neighbor].IsSupersetOf(clique))
                    {
                        clique.Add(neighbor);
                    }
                }
                if (clique.Count > largestClique.Count)
                {
                    largestClique = clique;
                }
            }
            return largestClique;
        }
    }
}