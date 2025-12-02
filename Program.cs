if (HasFlag("latest"))
{
    Solver.SolveLatest();
    return;
}

if (TryGetArgument("year", out var year) &&
    TryGetArgument("day", out var day))
{
    Solver.Solve(int.Parse(year), int.Parse(day));
    return;
}

return;

bool HasFlag(string flag)
{
    return IndexOfArgument("--" + flag) >= 0;
}

bool TryGetArgument(string argument, out string value)
{
    var index = IndexOfArgument("-" + argument);
    if (index >= 0 && index + 1 < args.Length)
    {
        value = args[index + 1];
        return true;
    }

    value = string.Empty;
    return false;
}

int IndexOfArgument(string argument)
{
    for (var i = 0; i < args.Length; i++)
    {
        if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
        {
            return i;
        }
    }

    return -1;
}