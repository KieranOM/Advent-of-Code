namespace AoC.Common;

public static class SolutionTypes
{
    public static Type? Latest()
    {
        return GetAll().LastOrDefault();
    }

    public static Type? Find(int year, int day)
    {
        foreach (var solution in GetAll())
        {
            ParseYearDay(solution, out var y, out var d);
            if (y == year && d == day)
            {
                return solution;
            }
        }

        return null;
    }

    private static IEnumerable<Type> GetAll()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(IsConcreteSolution)
            .OrderBy(type => type.FullName, StringComparer.Ordinal);
    }

    private static bool IsConcreteSolution(Type type)
    {
        return type.IsAssignableTo(typeof(SolutionBase)) &&
               type is { IsAbstract: false, IsInterface: false };
    }

    public static void ParseYearDay(Type solution, out int year, out int day)
    {
        var name = solution.FullName;
        var span = name.AsSpan();

        year = ParseIntFrom(span, ".Y", 4);
        day = ParseIntFrom(span, ".D", 2);
        return;

        static int ParseIntFrom(ReadOnlySpan<char> span, string pattern, int count)
        {
            var index = span.IndexOf(pattern);
            var sliced = span.Slice(index + pattern.Length, count);
            return int.Parse(sliced);
        }
    }
}