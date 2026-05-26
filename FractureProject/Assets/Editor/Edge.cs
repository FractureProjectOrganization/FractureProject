using UnityEngine;

public struct Edge
{
    public int v1;
    public int v2;

    public Edge(int a, int b)
    {
        v1 = Mathf.Min(a, b);
        v2 = Mathf.Max(a, b);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Edge)) return false;
        Edge other = (Edge)obj;
        return v1 == other.v1 && v2 == other.v2;
    }

    public override int GetHashCode()
    {
        return v1.GetHashCode() ^ v2.GetHashCode();
    }
}