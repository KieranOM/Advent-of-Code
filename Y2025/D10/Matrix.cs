using AoC.Toolkit.Numerics;

namespace AoC.Y2025.D10;

public class Matrix
{
    public int Rows { get; }
    public int Columns { get; }

    private readonly Fraction[,] _matrix;

    public Matrix(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        _matrix = new Fraction[rows, columns];

        for (var r = 0; r < rows; r++)
        for (var c = 0; c < columns; c++)
            _matrix[r, c] = Fraction.Zero;
    }

    public Fraction this[int i, int j]
    {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }
}