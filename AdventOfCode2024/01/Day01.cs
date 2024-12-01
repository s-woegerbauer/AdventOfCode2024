namespace AdventOfCode2024;

public static class Day01
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "01");
        var input = InputOutputHelper.GetInput(false, "01");
        
        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var leftList = input.Select(line => int.Parse(line.Split("   ")[0])).ToList();
        var rightList = input.Select(line => int.Parse(line.Split("   ")[1])).ToList();

        leftList.Sort();
        rightList.Sort();

        var totalDistance = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            totalDistance += Math.Abs(leftList[i] - rightList[i]);
        }

        InputOutputHelper.WriteOutput(isTest, totalDistance);
    }
    
    private static void PartTwo(bool isTest, string[] input)
    {
        var leftList = input.Select(line => int.Parse(line.Split("   ")[0])).ToList();
        var rightList = input.Select(line => int.Parse(line.Split("   ")[1])).ToList();

        var rightCount = new Dictionary<int, int>();
        foreach (var number in rightList)
        {
            if (rightCount.ContainsKey(number))
            {
                rightCount[number]++;
            }
            else
            {
                rightCount[number] = 1;
            }
        }

        var similarityScore = 0;
        foreach (var number in leftList)
        {
            if (rightCount.ContainsKey(number))
            {
                similarityScore += number * rightCount[number];
            }
        }

        InputOutputHelper.WriteOutput(isTest, similarityScore);
    }
}