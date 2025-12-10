namespace AoC.Y2025.D09;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var points = ReadInputAsLines()
            .Select(Vector2I.Parse)
            .ToList();

        var rectangles = new List<Rectangle>(points.Count * (points.Count - 1) / 2);

        for (var i = 0; i < points.Count; i++)
        for (var j = i + 1; j < points.Count; j++)
        {
            var rectangle = Rectangle.FromPoints(points[i], points[j]);
            rectangles.Add(rectangle);
        }

        rectangles.Sort(ByLargestArea);
        var largest = rectangles.First();
        Log(largest.Area);

        var poly = new Polygon(points);
        largest = rectangles.First(poly.Contains);
        Log(largest.Area);

        return;

        static int ByLargestArea(Rectangle left, Rectangle right) => right.Area.CompareTo(left.Area);
    }

    public readonly record struct Rectangle(int Left, int Bottom, int Right, int Top)
    {
        public readonly long Area = (Right - Left + 1L) * (Top - Bottom + 1L);

        public bool CollidesWith(in Rectangle other)
        {
            return (other.Right <= Left || // Left
                    other.Left >= Right || // Right
                    other.Top <= Bottom || // Below
                    other.Bottom >= Top) // Above
                   == false;
        }

        public static Rectangle FromPoints(in Vector2I first, in Vector2I second)
        {
            var top = Math.Max(first.Y, second.Y);
            var bottom = Math.Min(first.Y, second.Y);

            var right = Math.Max(first.X, second.X);
            var left = Math.Min(first.X, second.X);

            return new Rectangle(left, bottom, right, top);
        }
    }

    private class Polygon
    {
        private readonly List<Rectangle> _boundaries;

        public Polygon(List<Vector2I> points)
        {
            _boundaries = new List<Rectangle>(points.Count);

            for (var i = 1; i < points.Count; i++)
                _boundaries.Add(Rectangle.FromPoints(points[i - 1], points[i]));

            _boundaries.Add(Rectangle.FromPoints(points[^1], points[0]));
        }

        public bool Contains(Rectangle rectangle)
        {
            foreach (var boundary in _boundaries)
                if (rectangle.CollidesWith(boundary))
                    return false;

            return true;
        }
    }

    public readonly record struct Vector2I(int X, int Y)
    {
        public static Vector2I Parse(string input) => Parse(input.AsSpan());

        public static Vector2I Parse(ReadOnlySpan<char> input)
        {
            var enumerator = input.Split(',');
            Span<int> xy = stackalloc int[3];
            for (var i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                xy[i] = int.Parse(input[enumerator.Current].Trim());
            }

            return new Vector2I(xy[0], xy[1]);
        }
    }
}