using AoC.Toolkit.Numerics;

namespace AoC.Y2025.D08;

public class Solution : SolutionBase
{
    public override void Run()
    {
        var junctions = ReadInputAsLines()
            .Select(Vector3L.Parse)
            .ToList();
        var connections = GetConnectionsByCloseness(junctions);
        var circuits = new DisjointSet<Vector3L>(junctions);

        Connect(connections, circuits, count: 1000);
        var result = circuits.Partitions
            .Select(partition => (long)partition.Count)
            .OrderDescending()
            .Take(3)
            .Aggregate((accumulator, count) => accumulator * count);

        Log(result);

        var connection = FindFinalConnection(connections, circuits);
        result = connection.First.X * connection.Second.X;

        Log(result);
    }

    private static PriorityQueue<Connection, long> GetConnectionsByCloseness(List<Vector3L> positions)
    {
        var pairs = new PriorityQueue<Connection, long>();

        for (var i = 0; i < positions.Count; i++)
        for (var j = i + 1; j < positions.Count; j++)
        {
            var pair = new Connection(positions[i], positions[j]);
            pairs.Enqueue(pair, pair.Distance);
        }

        return pairs;
    }

    private static void Connect(PriorityQueue<Connection, long> connections, DisjointSet<Vector3L> circuits, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var connection = connections.Dequeue();
            circuits.Union(connection.First, connection.Second);
        }
    }

    private static Connection FindFinalConnection(PriorityQueue<Connection, long> connections,
        DisjointSet<Vector3L> circuits)
    {
        var connection = default(Connection);
        while (circuits.PartitionCount > 1)
        {
            connection = connections.Dequeue();
            circuits.Union(connection.First, connection.Second);
        }

        return connection;
    }

    private readonly record struct Connection(Vector3L First, Vector3L Second)
    {
        public readonly long Distance = Vector3L.DistanceSquared(First, Second);
    }
}