namespace AoC.Y2025.D03;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var banks = ReadInputAsLines()
            .Select(line => line
                .Select(ToLong)
                .ToList())
            .ToList();

        var total = MaximumJoltage(banks, batteries: 2);
        Log(total);

        total = MaximumJoltage(banks, batteries: 12);
        Log(total);
    }

    private static long MaximumJoltage(List<List<long>> banks, int batteries)
    {
        return banks.Sum(bank => MaximumJoltage(bank, batteries));
    }

    private static long MaximumJoltage(List<long> bank, int batteries)
    {
        var start = 0;
        var joltage = 0L;

        for (var i = 0; i < batteries; i++)
        {
            var max = (Index: start, Value: bank[start]);
            var end = bank.Count - (batteries - i - 1);

            for (var j = start + 1; j < end; j++)
            {
                var battery = bank[j];
                if (battery <= max.Value) continue;

                max = (Index: j, Value: battery);
            }

            start = max.Index + 1;
            joltage = joltage * 10 + max.Value;
        }

        return joltage;
    }

    private static long ToLong(char value) => value - '0';
}