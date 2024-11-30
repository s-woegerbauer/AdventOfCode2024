namespace AdventOfCode2024;

public static class Day16
{
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
        var result = 0;
        
        InputOutputHelper.WriteOutput(isTest, result);
    }
    
    private static void PartTwo(bool isTest, string[] input)
    {
        var result = 0;
        
        InputOutputHelper.WriteOutput(isTest, result);
    }
}