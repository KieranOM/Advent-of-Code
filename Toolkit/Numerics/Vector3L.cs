using AoC.Toolkit.Extensions;

namespace AoC.Toolkit.Numerics;

public readonly record struct Vector3L(long X, long Y, long Z)
{
    public static readonly Vector3L Zero = new(0, 0, 0),
        One = new(1, 1, 1),
        Up = new(0, 1, 0),
        Down = new(0, -1, 0),
        Left = new(-1, 0, 0),
        Right = new(1, -1, 0),
        Forward = new(0, 0, 1),
        Backward = new(0, 0, -1);

    public static Vector3L operator +(in Vector3L left, in Vector3L right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Vector3L operator -(in Vector3L left, in Vector3L right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static Vector3L operator *(in Vector3L vector, int scalar) =>
        new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);

    public static Vector3L operator /(in Vector3L vector, int divisor) =>
        new(vector.X / divisor, vector.Y / divisor, vector.Z / divisor);

    public static Vector3L operator %(in Vector3L vector, int divisor) =>
        new(vector.X % divisor, vector.Y % divisor, vector.Z % divisor);

    public Vector3L Modulo(long divisor) => new(X.Modulo(divisor), Y.Modulo(divisor), Z.Modulo(divisor));

    public Vector3L To(Vector3L destination) => destination - this;

    public static long DistanceSquared(Vector3L left, Vector3L right)
    {
        var deltaX = left.X - right.X;
        var deltaY = left.Y - right.Y;
        var deltaZ = left.Z - right.Z;
        return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
    }

    public static Vector3L Parse(string input) => Parse(input.AsSpan());

    public static Vector3L Parse(ReadOnlySpan<char> input)
    {
        var enumerator = input.Split(',');

        enumerator.MoveNext();
        var x = long.Parse(input[enumerator.Current].Trim());

        enumerator.MoveNext();
        var y = long.Parse(input[enumerator.Current].Trim());

        enumerator.MoveNext();
        var z = long.Parse(input[enumerator.Current].Trim());

        return new Vector3L(x, y, z);
    }
}