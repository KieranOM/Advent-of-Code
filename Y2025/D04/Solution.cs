namespace AoC.Y2025.D04;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var rolls = ParseRolls(ReadInputAsLines());

        Pick(rolls, out var total);
        Log(total);

        while (Pick(rolls, out var picked))
            total += picked;

        Log(total);
    }

    private static bool Pick(in HashSet<Vector2I> rolls, out int count)
    {
        var picked = new List<Vector2I>(rolls.Count);

        foreach (var roll in rolls)
            if (IsPickable(roll, rolls))
                picked.Add(roll);

        rolls.ExceptWith(picked);

        count = picked.Count;
        return count > 0;
    }

    private static bool IsPickable(in Vector2I roll, in HashSet<Vector2I> rolls)
    {
        var total = 0;

        for (var x = roll.X - 1; x <= roll.X + 1; x++)
        for (var y = roll.Y - 1; y <= roll.Y + 1; y++)
        {
            var position = new Vector2I(x, y);
            if (rolls.Contains(position))
                total++;
        }

        return total <= 4;
    }

    private static HashSet<Vector2I> ParseRolls(in string[] input)
    {
        const char roll = '@';
        var rolls = new HashSet<Vector2I>();

        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input.Length; x++)
            if (input[y][x] == roll)
                rolls.Add(new Vector2I(x, y));

        return rolls;
    }

    private readonly record struct Vector2I(int X, int Y);
}