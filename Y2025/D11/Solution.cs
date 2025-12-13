namespace AoC.Y2025.D11;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var graph = ParseInput(ReadInputAsLines());
        var memo = new Dictionary<Traversal, long>();

        var direct = Paths("you", "out");
        Log(direct);

        var stops = Paths("svr", "dac") * Paths("dac", "fft") * Paths("fft", "out") +
                    Paths("svr", "fft") * Paths("fft", "dac") * Paths("dac", "out");
        Log(stops);

        return;

        long Paths(string from, string to) => CountPaths(graph, new Traversal(from, to), memo);
    }

    private static long CountPaths(DirectedGraph<string> graph, Traversal traversal, Dictionary<Traversal, long> memo)
    {
        if (memo.TryGetValue(traversal, out var cached))
            return cached;

        if (traversal.Start == traversal.End)
            return memo[traversal] = 1L;

        return memo[traversal] = graph.Out(traversal.Start)
            .Sum(connected => CountPaths(graph, traversal with { Start = connected }, memo));
    }

    private static DirectedGraph<string> ParseInput(string[] input)
    {
        var graph = new DirectedGraph<string>();
        foreach (var line in input)
        {
            var split = line.Split(": ");
            var from = split[0];
            var destinations = split[1].Split(' ');

            foreach (var destination in destinations)
            {
                graph.Connect(from, destination);
            }
        }

        return graph;
    }

    private readonly record struct Traversal(string Start, string End);
}