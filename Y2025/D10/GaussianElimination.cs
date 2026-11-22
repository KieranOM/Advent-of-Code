namespace AoC.Y2025.D10;

public static class GaussianElimination
{
    public static void Run(Matrix matrix, out List<int> dependents, out List<int> independents)
    {
        dependents = [];
        independents = [];

        var variables = matrix.Columns - 1;

        var row = 0;
        for (var column = 0; column < variables; column++)
        {
            if (TryFindPivotForColumn(matrix, column, row, out var rowToSwap) == false) continue;

            SwapRows(matrix, row, rowToSwap);
            NormaliseRowByColumn(matrix, row, column);
            EliminateColumnInOtherRows(matrix, column, row);

            dependents.Add(column);

            if (++row >= matrix.Rows) break;
        }

        for (var i = 0; i < variables; i++)
        {
            if (dependents.Contains(i)) continue;

            independents.Add(i);
        }
    }

    private static bool TryFindPivotForColumn(Matrix matrix, int column, int startRow, out int pivot)
    {
        for (var r = startRow; r < matrix.Rows; r++)
        {
            if (matrix[r, column].IsZero()) continue;

            pivot = r;
            return true;
        }

        pivot = -1;
        return false;
    }

    private static void SwapRows(Matrix matrix, int row1, int row2)
    {
        for (var c = 0; c < matrix.Columns; c++)
        {
            (matrix[row1, c], matrix[row2, c]) = (matrix[row2, c], matrix[row1, c]);
        }
    }

    private static void NormaliseRowByColumn(Matrix matrix, int row, int column)
    {
        var value = matrix[row, column];
        for (var c = 0; c < matrix.Columns; c++)
        {
            matrix[row, c] /= value;
        }
    }

    private static void EliminateColumnInOtherRows(Matrix matrix, int column, int row)
    {
        for (var r = 0; r < matrix.Rows; r++)
        {
            if (r == row) continue;

            var factor = matrix[r, column];
            for (var c = 0; c < matrix.Columns; c++)
            {
                matrix[r, c] -= factor * matrix[row, c];
            }
        }
    }

    public static bool TryValidateAssignments(Matrix matrix, List<int> dependents, List<int> independents,
        int[] assignments, out int total)
    {
        total = assignments.Sum();

        for (var row = 0; row < dependents.Count; row++)
        {
            var sum = matrix[row, matrix.Columns - 1];
            for (var i = 0; i < independents.Count; i++)
            {
                sum -= matrix[row, independents[i]] * assignments[i];
            }

            if (sum.IsNegative())
            {
                return false;
            }

            if (sum.IsWhole() == false)
            {
                return false;
            }

            sum = sum.Simplified();
            total += sum.Numerator;
        }

        return true;
    }
}