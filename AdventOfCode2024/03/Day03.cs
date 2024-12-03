using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public static class Day03
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "03");
        var input = InputOutputHelper.GetInput(false, "03");

        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var inputString = string.Join("", input);
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var matches = regex.Matches(inputString);

        var result = matches
            .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
            .Sum();

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var inputString = string.Join("", input);
        var regexMul = new Regex(@"mul\((\d+),(\d+)\)");
        var regexDo = new Regex(@"do\(\)");
        var regexDont = new Regex(@"don't\(\)");

        bool isEnabled = true;
        int result = 0;

        for (int i = 0; i < inputString.Length;)
        {
            var doMatch = regexDo.Match(inputString, i);
            var dontMatch = regexDont.Match(inputString, i);
            var mulMatch = regexMul.Match(inputString, i);

            var firstMatch = new[] { doMatch, dontMatch, mulMatch }
                .Where(match => match.Success)
                .OrderBy(match => match.Index)
                .FirstOrDefault();

            if (firstMatch == null)
            {
                break;
            }

            if (firstMatch == doMatch)
            {
                isEnabled = true;
            }
            else if (firstMatch == dontMatch)
            {
                isEnabled = false;
            }
            else if (firstMatch == mulMatch && isEnabled)
            {
                int a = int.Parse(mulMatch.Groups[1].Value);
                int b = int.Parse(mulMatch.Groups[2].Value);
                result += a * b;
            }

            i = firstMatch.Index + firstMatch.Length;
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }
}