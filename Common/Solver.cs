using System.Diagnostics;

namespace AoC.Common;

public static class Solver
{
    private static class Message
    {
        public const string Running = "Running...";
        public const string Elapsed = "Time elapsed: ";
    }

    public static void Solve(int year, int day)
    {
        var solution = SolutionTypes.Find(year, day);
        if (solution == null)
            return;

        Solve(solution);
    }

    public static void Solve<TSolution>() where TSolution : SolutionBase
    {
        Solve(typeof(TSolution));
    }

    public static void Solve(Type solution)
    {
        if (Activator.CreateInstance(solution) is not SolutionBase instance)
            return;

        Solve(instance);
    }

    public static void Solve(SolutionBase solution)
    {
        solution.Log(Message.Running);
        var stopwatch = Stopwatch.StartNew();

        solution.Run();

        stopwatch.Stop();
        solution.Log(Message.Elapsed + stopwatch.Elapsed);
    }

    public static void SolveLatest()
    {
        var latest = SolutionTypes.Latest();
        if (latest == null)
            return;

        Solve(latest);
    }
}