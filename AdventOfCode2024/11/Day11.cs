namespace AdventOfCode2024;

public static class Day11
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "11");
        var input = InputOutputHelper.GetInput(false, "11");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var stones = Array.ConvertAll(input[0].Split(' '), long.Parse);
        int blinks = 25;
        long result = GetStoneCountAfterBlinks(stones, blinks);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var stones = Array.ConvertAll(input[0].Split(' '), long.Parse);
        int blinks = 75;
        long result = GetStoneCountAfterBlinks(stones, blinks);

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static long GetStoneCountAfterBlinks(long[] stones, int blinks)
    {
        var stoneCounts = new Dictionary<long, long>();

        foreach (var stone in stones)
        {
            if (stoneCounts.ContainsKey(stone))
            {
                stoneCounts[stone]++;
            }
            else
            {
                stoneCounts[stone] = 1;
            }
        }

        for (int i = 0; i < blinks; i++)
        {
            var newStoneCounts = new Dictionary<long, long>();

            foreach (var kvp in stoneCounts)
            {
                long stone = kvp.Key;
                long count = kvp.Value;

                if (stone == 0)
                {
                    if (newStoneCounts.ContainsKey(1))
                    {
                        newStoneCounts[1] += count;
                    }
                    else
                    {
                        newStoneCounts[1] = count;
                    }
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    var stoneStr = stone.ToString();
                    int mid = stoneStr.Length / 2;
                    long left = long.Parse(stoneStr.Substring(0, mid));
                    long right = long.Parse(stoneStr.Substring(mid));

                    if (newStoneCounts.ContainsKey(left))
                    {
                        newStoneCounts[left] += count;
                    }
                    else
                    {
                        newStoneCounts[left] = count;
                    }

                    if (newStoneCounts.ContainsKey(right))
                    {
                        newStoneCounts[right] += count;
                    }
                    else
                    {
                        newStoneCounts[right] = count;
                    }
                }
                else
                {
                    long newStone = stone * 2024;

                    if (newStoneCounts.ContainsKey(newStone))
                    {
                        newStoneCounts[newStone] += count;
                    }
                    else
                    {
                        newStoneCounts[newStone] = count;
                    }
                }
            }

            stoneCounts = newStoneCounts;
        }

        return stoneCounts.Values.Sum();
    }
}