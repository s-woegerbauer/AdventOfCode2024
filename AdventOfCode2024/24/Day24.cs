using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    using Circuit = Dictionary<string, Gate>;

    record struct Gate(string Input1, string Operation, string Input2);

    public static class Day24
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "24");
            var input = InputOutputHelper.GetInput(false, "24");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var (inputs, circuit) = Parse(string.Join('\n', input));

            var outputLabels = from label in circuit.Keys where label.StartsWith("z") select label;

            var result = 0L;
            foreach (var label in outputLabels.OrderByDescending(label => label))
            {
                result = result * 2 + Evaluate(label, circuit, inputs);
            }

            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static int Evaluate(string label, Circuit circuit, Dictionary<string, int> inputs)
        {
            if (inputs.TryGetValue(label, out var result))
            {
                return result;
            }

            return circuit[label] switch
            {
                Gate(var input1, "AND", var input2) => Evaluate(input1, circuit, inputs) & Evaluate(input2, circuit, inputs),
                Gate(var input1, "OR", var input2) => Evaluate(input1, circuit, inputs) | Evaluate(input2, circuit, inputs),
                Gate(var input1, "XOR", var input2) => Evaluate(input1, circuit, inputs) ^ Evaluate(input2, circuit, inputs),
                _ => throw new Exception(circuit[label].ToString()),
            };
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var operations = new List<(string Operand1, string Operation, string Operand2, string Result)>();
            string highestZLabel = "z00";

            foreach (var line in input)
            {
                if (line.Contains("->"))
                {
                    var parts = line.Split(" ");
                    operations.Add((parts[0], parts[1], parts[2], parts[4]));
                    if (parts[4][0] == 'z' && int.Parse(parts[4].Substring(1)) > int.Parse(highestZLabel.Substring(1)))
                    {
                        highestZLabel = parts[4];
                    }
                }
            }

            var incorrectLabels = new HashSet<string>();

            foreach (var (operand1, operation, operand2, result) in operations)
            {
                if (result[0] == 'z' && operation != "XOR" && result != highestZLabel)
                {
                    incorrectLabels.Add(result);
                }

                if (operation == "XOR" && result[0] != 'x' && result[0] != 'y' && result[0] != 'z' && operand1[0] != 'x' && operand1[0] != 'y' &&
                    operand1[0] != 'z' && operand2[0] != 'x' && operand2[0] != 'y' && operand2[0] != 'z')
                {
                    incorrectLabels.Add(result);
                }

                if (operation == "AND" && operand1 != "x00" && operand2 != "x00")
                {
                    foreach (var (subOperand1, subOperation, subOperand2, subResult) in operations)
                    {
                        if ((result == subOperand1 || result == subOperand2) && subOperation != "OR")
                        {
                            incorrectLabels.Add(result);
                        }
                    }
                }

                if (operation == "XOR")
                {
                    foreach (var (subOperand1, subOperation, subOperand2, subResult) in operations)
                    {
                        if ((result == subOperand1 || result == subOperand2) && subOperation == "OR")
                        {
                            incorrectLabels.Add(result);
                        }
                    }
                }
            }

            InputOutputHelper.WriteOutput(isTest, string.Join(",", incorrectLabels.OrderBy(label => label)));
        }

        private static (Dictionary<string, int> inputs, Circuit circuit) Parse(string input)
        {
            var inputs = new Dictionary<string, int>();
            var circuit = new Circuit();

            var lines = input.Split("\n");

            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(": ");
                    inputs.Add(parts[0], int.Parse(parts[1]));
                }
                else if (line.Contains("->"))
                {
                    var parts = Regex.Matches(line, "[a-zA-z0-9]+").Select(m => m.Value).ToArray();
                    circuit.Add(parts[3], new Gate(parts[0], parts[1], parts[2]));
                }
            }

            return (inputs, circuit);
        }
    }
}