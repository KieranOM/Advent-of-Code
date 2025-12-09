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

    private readonly record struct Vector3L(long X, long Y, long Z)
    {
        public static long DistanceSquared(Vector3L left, Vector3L right)
        {
            var deltaX = left.X - right.X;
            var deltaY = left.Y - right.Y;
            var deltaZ = left.Z - right.Z;
            return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
        }

        public static Vector3L Parse(string input) => Parse(input.AsSpan());

        public static Vector3L Parse(ReadOnlySpan<char> input)
        {
            var enumerator = input.Split(',');
            Span<long> xyz = stackalloc long[3];
            for (var i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                xyz[i] = long.Parse(input[enumerator.Current].Trim());
            }

            return new Vector3L(xyz[0], xyz[1], xyz[2]);
        }
    }
}