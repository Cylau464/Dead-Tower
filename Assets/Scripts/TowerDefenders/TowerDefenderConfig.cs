using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Tower Defender", menuName = "Tower/Defender")]
public class TowerDefenderConfig : ScriptableObject
{
    public int Index;
    public SkeletonDataAsset Skeleton;
    public RuntimeAnimatorController AnimatorController;
    public Projectile ProjectilePrefab;
    public ModifierConfig Modifier;
    public TowerDefenderStats Stats;
    public PurchaseStats PurchaseStats;
}

public class DefenderData
{
    public int Index;
    public TowerDefenderStats Stats;
    public bool IsPurchased;

    public DefenderData(int index, TowerDefenderStats stats, bool isPurchased)
    {
        Index = index;
        Stats = stats;
        IsPurchased = isPurchased;
    }
}