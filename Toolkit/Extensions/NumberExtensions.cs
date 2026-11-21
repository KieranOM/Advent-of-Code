using System.Numerics;

namespace AoC.Toolkit.Extensions;

public static class NumberExtensions
{
    public static TNumber Modulo<TNumber>(this TNumber number, TNumber divisor) where TNumber : INumber<TNumber>
    {
        return (number % divisor + divisor) % divisor;
    }
}