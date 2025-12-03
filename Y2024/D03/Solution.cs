using System.Text.RegularExpressions;

namespace AoC.Y2024.D03;

public partial class Solution : SolutionBase
{
    private readonly Regex _multiplicationPattern = MultiplicationRegex();
    private readonly Regex _disabledPattern = DisabledRegex();

    public override void Run()
    {
        var input = ReadInputAsLines();
        string memory = string.Join("", input).Trim();

        int sum = SumMultiplications(memory);
        Log(sum);

        string enabledMemory = _disabledPattern.Replace(memory + "do()", "");
        int enabledSum = SumMultiplications(enabledMemory);
        Log(enabledSum);
    }

    private int SumMultiplications(in string memory)
    {
        return _multiplicationPattern.Matches(memory)
            .Sum(ParseMultiplication);
    }

    private static int ParseMultiplication(Match match)
    {
        int left = int.Parse(match.Groups[1].Value);
        int right = int.Parse(match.Groups[2].Value);
        return left * right;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MultiplicationRegex();

    [GeneratedRegex(@"don't\(\).*?do\(\)")]
    private static partial Regex DisabledRegex();
}