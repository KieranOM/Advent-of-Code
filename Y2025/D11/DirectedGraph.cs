namespace AoC.Y2025.D11;

public class DirectedGraph<T> where T : IEquatable<T>
{
    private readonly Dictionary<T, HashSet<T>> _in = new();
    private readonly Dictionary<T, HashSet<T>> _out = new();

    public IEnumerable<T> In(T node) => GetOrEmpty(node, _in);
    public IEnumerable<T> Out(T node) => GetOrEmpty(node, _out);

    private static HashSet<T> GetOrEmpty(T node, Dictionary<T, HashSet<T>> direction)
    {
        if (direction.TryGetValue(node, out var set) == false)
        {
            return direction[node] = [];
        }

        return set;
    }

    public void Connect(T from, T to)
    {
        GetOrEmpty(to, _in).Add(from);
        GetOrEmpty(from, _out).Add(to);
    }

    public void Disconnect(T from, T to)
    {
        GetOrEmpty(to, _in).Remove(from);
        GetOrEmpty(from, _out).Remove(to);
    }
}