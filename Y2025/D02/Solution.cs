using System.Collections;

namespace AoC.Y2025.D02;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var ranges = ReadInputAsText()
            .Split(',')
            .Select(Range.Parse)
            .ToList();

        var invalid = SumInvalidIds(ranges, strategy: TwoRepeats);
        Log(invalid);

        invalid = SumInvalidIds(ranges, strategy: AtLeastTwoRepeats);
        Log(invalid);
    }

    private static long SumInvalidIds(List<Range> ranges, Predicate<ReadOnlySpan<char>> strategy)
    {
        return ranges.SelectMany(range => range)
            .Where(id => IsInvalidId(id, strategy))
            .Sum();
    }

    private static bool IsInvalidId(long numericalId, Predicate<ReadOnlySpan<char>> strategy)
    {
        const int maxLongDigits = 19; // Floor(Log10(long.MaxValue)) + 1

        Span<char> span = stackalloc char[maxLongDigits];
        numericalId.TryFormat(span, out var length);

        var id = span[..length];
        return strategy(id);
    }

    private static bool IsInvalidId(ReadOnlySpan<char> id, int repeats)
    {
        var length = id.Length;
        if (length % repeats != 0)
            return false;

        var chunk = length / repeats;
        var sequence = id[..chunk];

        for (var i = chunk; i < length; i += chunk)
        {
            var remaining = id[i..];
            if (remaining.StartsWith(sequence) == false)
                return false;
        }

        return true;
    }

    private static bool TwoRepeats(ReadOnlySpan<char> id)
    {
        return IsInvalidId(id, repeats: 2);
    }

    private static bool AtLeastTwoRepeats(ReadOnlySpan<char> id)
    {
        for (var i = 2; i <= id.Length; i++)
            if (IsInvalidId(id, repeats: i))
                return true;
        return false;
    }

    private readonly record struct Range(long Start, long End) : IEnumerable<long>
    {
        public static Range Parse(string s) => Parse(s.AsSpan());

        public static Range Parse(ReadOnlySpan<char> span)
        {
            var split = span.IndexOf('-');
            return new Range(
                Start: long.Parse(span[..split]),
                End: long.Parse(span[(split + 1)..])
            );
        }

        public IEnumerator<long> GetEnumerator()
        {
            for (var i = Start; i <= End; i++)
                yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}