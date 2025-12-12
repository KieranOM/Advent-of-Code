namespace AoC.Y2025.D10;

public class Matrix(int rows, int columns)
{
    public int Rows => rows;
    public int Columns => columns;

    private readonly double[,] _matrix = new double[rows, columns];

    public double this[int i, int j]
    {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }
}