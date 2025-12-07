namespace AoC.Y2025.D07;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var grid = ParseGrid(ReadInputAsLines());

        var splits = CountSplits(grid);
        Log(splits);

        var timelines = CountTimelines(grid, grid.Start, new Dictionary<Vector2I, long>());
        Log(timelines);
    }

    private static int CountSplits(Grid grid)
    {
        var beams = new Queue<Vector2I>();
        Enqueue(grid.Start);
        var count = 0;

        while (beams.Count > 0)
        {
            var beam = beams.Dequeue() + Vector2I.Up;
            if (grid.Contains(beam) == false) continue;

            if (grid.Splitters.Contains(beam))
            {
                count++;
                Enqueue(beam + Vector2I.Left);
                Enqueue(beam + Vector2I.Right);
                continue;
            }

            Enqueue(beam);
        }

        return count;

        void Enqueue(in Vector2I beam)
        {
            if (beams.Contains(beam) == false)
                beams.Enqueue(beam);
        }
    }

    private static long CountTimelines(Grid grid, Vector2I particle, Dictionary<Vector2I, long> memo)
    {
        if (memo.TryGetValue(particle, out var timelines))
            return timelines;

        particle += Vector2I.Up;
        if (grid.Contains(particle) == false)
            return memo[particle] = 1;

        if (grid.Splitters.Contains(particle))
            return memo[particle] = CountTimelines(grid, particle + Vector2I.Left, memo) +
                                    CountTimelines(grid, particle + Vector2I.Right, memo);

        return memo[particle] = CountTimelines(grid, particle, memo);
    }


    private readonly record struct Vector2I(int X, int Y)
    {
        public static Vector2I Up { get; } = new(0, 1);
        public static Vector2I Left { get; } = new(-1, 0);
        public static Vector2I Right { get; } = new(1, 0);

        public static Vector2I operator +(in Vector2I left, in Vector2I right) =>
            new(left.X + right.X, left.Y + right.Y);
    }

    private static Grid ParseGrid(string[] inputs)
    {
        const char start = 'S', splitter = '^';

        var width = inputs[0].Length;
        var height = inputs.Length;
        var grid = new Grid(width, height);

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var position = new Vector2I(x, y);
            var cell = inputs[y][x];
            if (cell == start)
            {
                grid.Start = position;
            }
            else if (cell == splitter)
            {
                grid.Splitters.Add(position);
            }
        }

        return grid;
    }

    private class Grid(int width, int height)
    {
        public Vector2I Start;
        public readonly HashSet<Vector2I> Splitters = [];

        public bool Contains(in Vector2I position)
        {
            return position.X >= 0 && position.X < width &&
                   position.Y >= 0 && position.Y < height;
        }
    }
}