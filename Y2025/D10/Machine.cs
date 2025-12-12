namespace AoC.Y2025.D10;

public class Machine(List<int> lights, List<List<int>> buttons, List<int> joltages)
{
    public readonly List<int> Lights = lights;
    public readonly List<List<int>> Buttons = buttons;
    public readonly List<int> Joltages = joltages;

    public readonly int LightsMask = ToMask(lights);
    public readonly List<int> ButtonsAsMasks = buttons.Select(ToMask).ToList();

    public Matrix ToEquationSystemMatrix()
    {
        var variables = Buttons.Count;
        var matrix = new Matrix(Joltages.Count, variables + 1);

        // Buttons as columns for variables
        for (int column = 0; column < Buttons.Count; column++)
        {
            foreach (var row in Buttons[column])
            {
                matrix[row, column] = 1.0;
            }
        }

        // Joltages as final column for solutions
        for (var row = 0; row < matrix.Rows; row++)
        {
            matrix[row, variables] = Joltages[row];
        }

        return matrix;
    }

    public static Machine Parse(string input)
    {
        var split = input.Split(' ');

        var lights = split[0][1..^1]
            .Index()
            .Where(c => c.Item == '#')
            .Select(c => c.Index)
            .ToList();

        var buttons = split[1..^1]
            .Select(brackets => brackets[1..^1])
            .Select(buttons => buttons.Split(',')
                .Select(int.Parse)
                .ToList()
            )
            .ToList();

        var joltages = split[^1][1..^1]
            .Split(',')
            .Select(int.Parse)
            .ToList();

        return new Machine(lights, buttons, joltages);
    }

    private static int ToMask(List<int> bits) => bits.Sum(bit => 1 << bit);
}