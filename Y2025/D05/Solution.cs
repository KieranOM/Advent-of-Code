namespace AoC.Y2025.D05;

public class Solution : SolutionBase
{
    public override void Run()
    {
        ParseInput(ReadInputAsLines(), out var ranges, out var ingredients);
        ranges.Sort();
        ranges = Merge(ranges);

        var total = 0;
        foreach (var ingredient in ingredients)
            if (IsValid(ingredient, ranges))
                total++;

        Log(total);

        var unique = 0L;
        foreach (var range in ranges)
            unique += range.Values;

        Log(unique);
    }

    private static List<Range> Merge(List<Range> ranges)
    {
        var result = new List<Range> { ranges[0] };

        for (var i = 1; i < ranges.Count; i++)
        {
            var first = result[^1];
            var second = ranges[i];

            if (TryMerge(first, second, out var merged))
                result[^1] = merged;
            else
                result.Add(second);
        }

        return result;
    }

    private static bool TryMerge(in Range earlier, in Range later, out Range merged)
    {
        if (later.Start - earlier.End <= 1)
        {
            merged = new Range(Math.Min(earlier.Start, later.Start), Math.Max(earlier.End, later.End));
            return true;
        }

        merged = default;
        return false;
    }

    private static bool IsValid(long ingredient, List<Range> ranges)
    {
        foreach (var range in ranges)
            if (range.Contains(ingredient))
                return true;
        return false;
    }

    private static void ParseInput(string[] input, out List<Range> ranges, out List<long> ingredients)
    {
        var split = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        var span = input.AsSpan();

        ranges = ParseRanges(span[..split]);
        ingredients = ParseIngredients(span[(split + 1)..]);
    }

    private static List<Range> ParseRanges(in ReadOnlySpan<string> input)
    {
        var ranges = new List<Range>(input.Length);
        foreach (var line in input)
            ranges.Add(Range.Parse(line));
        return ranges;
    }

    private static List<long> ParseIngredients(in ReadOnlySpan<string> input)
    {
        var ingredients = new List<long>(input.Length);
        foreach (var line in input)
            ingredients.Add(long.Parse(line));
        return ingredients;
    }

    private readonly record struct Range(long Start, long End) : IComparable<Range>
    {
        public long Values => End - Start + 1;
        public bool Contains(long value) => value >= Start && value <= End;

        public int CompareTo(Range other) => Start.CompareTo(other.Start);

        public static Range Parse(in ReadOnlySpan<char> span)
        {
            var split = span.IndexOf('-');
            return new Range(long.Parse(span[..split]), long.Parse(span[(split + 1)..]));
        }
    }
}