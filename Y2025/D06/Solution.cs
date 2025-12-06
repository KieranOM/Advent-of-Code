namespace AoC.Y2025.D06;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var worksheet = ReadInputAsLines();

        var total = SolveVertical(worksheet);
        Log(total);

        total = SolveRightToLeft(worksheet);
        Log(total);
    }

    private static long Solve(IEnumerable<long> operands, char operation)
    {
        return operation == '*'
            ? operands.Aggregate(1L, (accumulator, operand) => accumulator * operand)
            : operands.Aggregate(0L, (accumulator, operand) => accumulator + operand);
    }

    private static long SolveVertical(string[] worksheet)
    {
        var operands = worksheet[..^1]
            .Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList())
            .ToList();

        var operations = worksheet[^1]
            .Where(c => char.IsWhiteSpace(c) == false)
            .ToList();

        return operations.Select(SolveColumn)
            .Sum();

        long SolveColumn(char operation, int column)
        {
            return Solve(operands.Select(row => row[column]), operation);
        }
    }

    private static long SolveRightToLeft(string[] worksheet)
    {
        var rows = worksheet.Length;
        var columns = worksheet[0].Length;
        var operations = worksheet[rows - 1];

        var total = 0L;
        var operands = new List<long>();

        for (var column = columns - 1; column >= 0; column--)
        {
            var operand = 0L;
            for (var row = 0; row < rows - 1; row++)
            {
                if (char.IsWhiteSpace(worksheet[row][column])) continue;

                var digit = worksheet[row][column] - '0';
                operand = operand * 10 + digit;
            }

            operands.Add(operand);

            var operation = operations[column];
            if (char.IsWhiteSpace(operation)) continue;

            total += Solve(operands, operation);
            operands.Clear();

            column--;
        }

        return total;
    }
}