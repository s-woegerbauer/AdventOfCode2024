﻿namespace AdventOfCode2024;

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
        var result = 0;
        
        InputOutputHelper.WriteOutput(isTest, result);
    }
    
    private static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;
        
        InputOutputHelper.WriteOutput(isTest, result);
    }
}