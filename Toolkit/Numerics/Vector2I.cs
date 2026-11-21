using AoC.Toolkit.Extensions;

namespace AoC.Toolkit.Numerics;

public readonly record struct Vector2I(int X, int Y)
{
    public static readonly Vector2I Zero = new(0, 0),
        One = new(1, 1),
        Up = new(0, 1),
        Down = new(0, -1),
        Left = new(-1, 0),
        Right = new(1, 0);

    public static Vector2I operator +(in Vector2I left, in Vector2I right) => new(left.X + right.X, left.Y + right.Y);
    public static Vector2I operator -(in Vector2I left, in Vector2I right) => new(left.X - right.X, left.Y - right.Y);
    public static Vector2I operator *(in Vector2I vector, int scalar) => new(vector.X * scalar, vector.Y * scalar);
    public static Vector2I operator /(in Vector2I vector, int divisor) => new(vector.X / divisor, vector.Y / divisor);
    public static Vector2I operator %(in Vector2I vector, int divisor) => new(vector.X % divisor, vector.Y % divisor);

    public Vector2I Modulo(int divisor) => new(X.Modulo(divisor), Y.Modulo(divisor));

    public Vector2I To(Vector2I destination) => destination - this;

    public Vector2I Skew(bool clockwise = false) => clockwise
        ? new Vector2I(Y, -X)
        : new Vector2I(-Y, X);

    public static int ManhattanDistance(in Vector2I from, in Vector2I to) =>
        Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

    public IEnumerable<Vector2I> Adjacent()
    {
        yield return this + Up;
        yield return this + Down;
        yield return this + Left;
        yield return this + Right;
    }

    public static Vector2I Parse(string input) => Parse(input.AsSpan());

    public static Vector2I Parse(ReadOnlySpan<char> input)
    {
        var enumerator = input.Split(',');

        enumerator.MoveNext();
        var x = int.Parse(input[enumerator.Current].Trim());

        enumerator.MoveNext();
        var y = int.Parse(input[enumerator.Current].Trim());

        return new Vector2I(x, y);
    }
}