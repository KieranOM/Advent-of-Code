namespace AoC.Y2025.D01;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var rotations = ReadInputAsLines()
            .Select(Rotation.Parse)
            .ToList();

        var rests = CountZeroes(rotations, includePasses: false);
        Log(rests);

        var restsAndPasses = CountZeroes(rotations, includePasses: true);
        Log(restsAndPasses);
    }

    private static int CountZeroes(List<Rotation> rotations, bool includePasses)
    {
        const int start = 50, positions = 100;
        var position = start;
        var count = 0;

        foreach (var rotation in rotations)
        {
            var previous = position;
            position = Wrap(position + rotation.Value, positions);

            if (includePasses)
            {
                count += rotation.Clicks / positions;

                if (rotation.Clockwise && position < previous && position != 0 ||
                    rotation.Clockwise == false && position > previous && previous != 0)
                    count++;
            }

            if (position == 0)
                count++;
        }

        return count;
    }

    private static int Wrap(int number, int limit) => (number % limit + limit) % limit;

    private readonly record struct Rotation(bool Clockwise, int Clicks)
    {
        public int Value => Clockwise ? Clicks : -Clicks;

        public static Rotation Parse(string s) => new(s[0] == 'R', int.Parse(s[1..]));
    }
}