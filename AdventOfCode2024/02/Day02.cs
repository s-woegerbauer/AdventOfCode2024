namespace AdventOfCode2024;

public static class Day02
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "02");
        var input = InputOutputHelper.GetInput(false, "02");
        
        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var reports = input
            .Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();

        int safeCount = 0;

        foreach (var report in reports)
        {
            if (IsSafeReport(report))
            {
                safeCount++;
            }
        }

        InputOutputHelper.WriteOutput(isTest, safeCount);
    }

    static bool IsSafeReport(List<int> report)
    {
        int previous = -1;
        bool isIncreasing = true;
        bool checkedIncreasing = false;
        
        foreach (var level in report)
        {
            if(previous == -1)
            {
                previous = level;
                continue;
            }

            if (!checkedIncreasing)
            {
                if(level > previous)
                {
                    isIncreasing = true;
                    checkedIncreasing = true;
                }
                else if(level < previous)
                {
                    isIncreasing = false;
                    checkedIncreasing = true;
                }
            }

            if (isIncreasing)
            {
                if (level - previous < 1 || level - previous > 3)
                {
                    return false;
                }
            }
            else
            {
                if (previous - level < 1 || previous - level > 3)
                {
                    return false;
                }
            }
            
            previous = level;
        }
        
        return true;
    }


    private static void PartTwo(bool isTest, string[] input)
    {
        var reports = input
            .Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();

        int safeCount = 0;

        foreach (var report in reports)
        {
            if (IsSafeReport(report) || CanBeSafeWithOneRemoval(report))
            {
                safeCount++;
            }
        }

        InputOutputHelper.WriteOutput(isTest, safeCount);
    }
    
    static bool CanBeSafeWithOneRemoval(List<int> report)
    {
        for (int i = 0; i < report.Count; i++)
        {
            var modifiedReport = new List<int>(report);
            modifiedReport.RemoveAt(i);

            if (IsSafeReport(modifiedReport))
            {
                return true;
            }
        }

        return false;
    }
}