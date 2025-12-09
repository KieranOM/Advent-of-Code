namespace AoC.Y2025.D08;

public class DisjointSet<T> where T : IEquatable<T>
{
    private readonly Dictionary<T, HashSet<T>> _partitions = new();

    public DisjointSet()
    {
    }

    public DisjointSet(IEnumerable<T> collection)
    {
        foreach (var item in collection)
            MakeSet(item);
    }

    public int PartitionCount { get; private set; } = 0;
    public IEnumerable<HashSet<T>> Partitions => _partitions.Values.Distinct();

    private void MakeSet(T item)
    {
        _partitions[item] = [item];
        PartitionCount++;
    }

    public bool Union(T a, T b)
    {
        var setA = _partitions[a];
        var setB = _partitions[b];

        if (setA == setB)
            return false;

        if (setB.Count > setA.Count)
            (setA, setB) = (setB, setA);

        foreach (var item in setB)
            _partitions[item] = setA;

        setA.UnionWith(setB);
        PartitionCount--;
        return true;
    }

    public void Clear()
    {
        _partitions.Clear();
        PartitionCount = 0;
    }
}