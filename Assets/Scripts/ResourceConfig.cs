using UnityEngine;
public enum ResourceTypes { Wood, Iron, Stone }

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
}