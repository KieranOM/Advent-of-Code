namespace AoC.Toolkit.Numerics;

public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
{
    public static readonly Fraction Zero = new(0);
    public static readonly Fraction One = new(1);

    public readonly int Numerator;
    public readonly int Denominator;

    public Fraction(int numerator, int denominator = 1)
    {
        if (denominator == 0)
            throw new DivideByZeroException();

        Numerator = numerator;
        Denominator = denominator;
    }

    public static implicit operator Fraction(int value)
    {
        return new Fraction(value);
    }

    public static explicit operator double(Fraction value)
    {
        return (double)value.Numerator / value.Denominator;
    }

    public static bool operator ==(Fraction left, Fraction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Fraction left, Fraction right)
    {
        return !(left == right);
    }

    public static bool operator <(Fraction left, Fraction right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(Fraction left, Fraction right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(Fraction left, Fraction right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(Fraction left, Fraction right)
    {
        return left.CompareTo(right) >= 0;
    }

    public Fraction Reciprocal()
    {
        return new Fraction(Denominator, Numerator);
    }

    public bool IsWhole() => Numerator % Denominator == 0;

    public bool IsPositive() => int.IsPositive(Numerator) == int.IsPositive(Denominator);
    public bool IsNegative() => int.IsNegative(Numerator) != int.IsNegative(Denominator);
    public bool IsZero() => Numerator == 0;

    public int CompareTo(Fraction other)
    {
        return ((long)Numerator * other.Denominator).CompareTo((long)Denominator * other.Numerator);
    }

    public bool Equals(Fraction other)
    {
        return CompareTo(other) == 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is Fraction other && Equals(other);
    }

    public static Fraction operator +(Fraction fraction)
    {
        return fraction;
    }

    public static Fraction operator -(Fraction fraction)
    {
        return new Fraction(-fraction.Numerator, fraction.Denominator);
    }

    public static Fraction operator +(Fraction left, Fraction right)
    {
        var numerator = (long)left.Numerator * right.Denominator + (long)left.Denominator * right.Numerator;
        var denominator = (long)left.Denominator * right.Denominator;
        return FromLongs(numerator, denominator);
    }

    public static Fraction operator -(Fraction left, Fraction right)
    {
        return left + -right;
    }

    public static Fraction operator *(Fraction left, Fraction right)
    {
        var numerator = (long)left.Numerator * right.Numerator;
        var denominator = (long)left.Denominator * right.Denominator;
        return FromLongs(numerator, denominator);
    }

    public static Fraction operator /(Fraction left, Fraction right)
    {
        return left * right.Reciprocal();
    }

    public override int GetHashCode()
    {
        var simplified = Simplified();
        return HashCode.Combine(simplified.Numerator, simplified.Denominator);
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public Fraction Simplified()
    {
        if (Numerator == 0)
        {
            return Zero;
        }

        if (Denominator == 1)
        {
            return this;
        }

        var gcd = (int)GreatestCommonDenominator(Numerator, Denominator);
        var denominator = Denominator / gcd;
        var numerator = Numerator / gcd;
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        return new Fraction(numerator, denominator);
    }

    private static Fraction FromLongs(long numerator, long denominator)
    {
        if (numerator == 0)
        {
            return Zero;
        }

        var gcd = GreatestCommonDenominator(numerator, denominator);

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        return new Fraction((int)(numerator / gcd), (int)(denominator / gcd));
    }

    private static long GreatestCommonDenominator(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);

        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }
}