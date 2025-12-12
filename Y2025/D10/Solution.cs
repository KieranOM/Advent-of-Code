namespace AoC.Y2025.D10;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var machines = ReadInputAsLines()
            .Select(Machine.Parse)
            .ToList();

        var pressesForLights = machines
            .Sum(MinimumPressesForLightState);
        Log(pressesForLights);

        var pressesForJoltages = machines
            .AsParallel()
            .Sum(MinimumPressesForJoltages);
        Log(pressesForJoltages);
    }

    private static int MinimumPressesForLightState(Machine machine)
    {
        var target = machine.LightsMask;
        var seen = new HashSet<int>();
        var queue = new Queue<(int lights, int presses)>();
        queue.Enqueue((0, 0));

        while (queue.TryDequeue(out var state))
        {
            if (state.lights == target)
                return state.presses;

            foreach (var mask in machine.ButtonsAsMasks)
            {
                var next = state.lights ^ mask;
                if (seen.Add(next))
                {
                    queue.Enqueue((next, state.presses + 1));
                }
            }
        }

        return -1;
    }

    private static int MinimumPressesForJoltages(Machine machine)
    {
        var matrix = machine.ToEquationSystemMatrix();
        var domains = CalculateVariableDomains(matrix);

        GaussianElimination.Run(matrix, out var dependents, out var independents);

        return FindMinimumTotalAssignments(matrix, domains, dependents, independents);
    }

    private static int[] CalculateVariableDomains(Matrix matrix)
    {
        var domains = new int[matrix.Columns - 1];
        for (var i = 0; i < matrix.Columns - 1; i++)
        {
            domains[i] = CalculateVariableDomain(matrix, i);
        }

        return domains;
    }

    private static int CalculateVariableDomain(Matrix matrix, int variable)
    {
        var min = int.MaxValue;
        var solution = matrix.Columns - 1;
        for (var row = 0; row < matrix.Rows; row++)
        {
            if (matrix[row, variable] == 0.0d) continue;

            min = Math.Min(min, (int)matrix[row, solution]);
        }

        return min;
    }

    public static int FindMinimumTotalAssignments(Matrix matrix,
        int[] domains,
        List<int> dependents,
        List<int> independents)
    {
        var min = int.MaxValue;
        var assignments = new int[independents.Count];

        SearchIndependents(matrix, domains, dependents, independents, 0, assignments, ref min, 0);
        return min;
    }

    private static void SearchIndependents(Matrix matrix,
        int[] domains,
        List<int> dependents,
        List<int> independents,
        int index,
        int[] assignments,
        ref int min,
        int presses)
    {
        if (index == independents.Count)
        {
            if (GaussianElimination.TryValidateAssignments(matrix, dependents, independents, assignments,
                    out var total))
            {
                min = Math.Min(min, total);
            }

            return;
        }

        var domain = domains[independents[index]];
        for (var assignment = 0; assignment <= domain; assignment++)
        {
            var next = presses + assignment;
            if (next >= min) break;

            assignments[index] = assignment;
            SearchIndependents(matrix, domains, dependents, independents, index + 1, assignments, ref min, next);
        }
    }
}