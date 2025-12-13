namespace AoC.Y2025.D12;

public class Solution : SolutionBase
{
    public override void Run()
    {
        ParseInput(ReadInputAsText(), out var presents, out var regions);

        var result = CountPackableRegions(regions, presents);
        Log(result);
    }

    private static int CountPackableRegions(List<Region> regions, List<Present> presents)
    {
        return regions.Count(region => CanPack(region, presents));
    }

    private static bool CanPack(Region region, List<Present> presents)
    {
        return IsLargeEnough(region, presents);
    }

    private static bool IsLargeEnough(Region region, List<Present> presents)
    {
        var remaining = region.Area;
        for (var i = 0; i < region.Quantities.Count; i++)
        {
            remaining -= region.Quantities[i] * presents[i].Area;
        }

        return remaining >= 0;
    }

    private static void ParseInput(string input, out List<Present> presents, out List<Region> regions)
    {
        presents = [];
        regions = [];

        var split = input.Split([Environment.NewLine + Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        presents = split[..^1].Select(Present.Parse).ToList();
        regions = split[^1].Split(Environment.NewLine).Select(Region.Parse).ToList();
    }

    private readonly record struct Present(int Area)
    {
        public static Present Parse(string input) => new(Area: input.AsSpan().Count('#'));
    }

    private readonly record struct Region(int Width, int Height, List<int> Quantities)
    {
        public readonly int Area = Width * Height;

        public static Region Parse(string input)
        {
            var split = input.Split(": ");

            var dimensions = split[0].Split('x');
            var width = int.Parse(dimensions[0]);
            var height = int.Parse(dimensions[1]);

            var presents = split[1].Split(' ').Select(int.Parse).ToList();
            return new Region(width, height, presents);
        }
    }
}