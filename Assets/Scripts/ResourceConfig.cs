using UnityEngine;
public enum ResourceTypes { None, Wood, Iron, Stone }

[CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
public class ResourceConfig : ScriptableObject
{
    public ResourceTypes Type;
    public Sprite Icon;
    public Sprite ForgeIcon;
    public Sprite RewardIcon;
    public Sprite ShopIcon;
}

[System.Serializable]
public struct Resource
{
    public ResourceTypes Type;
    public int Count;

    public Resource(ResourceTypes type, int count)
    {
        Type = type;
        Count = count;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static Resource operator +(Resource a, Resource b)
        => new Resource(a.Type, a.Count + b.Count);

    public static bool operator ==(Resource a, Resource b)
    {
        if (a.Type == b.Type && a.Count == b.Count)
            return true;
        else
            return false;
    }

    public static bool operator !=(Resource a, Resource b)
    {
        if (a.Type == b.Type && a.Count == b.Count)
            return false;
        else
            return true;
    }
}