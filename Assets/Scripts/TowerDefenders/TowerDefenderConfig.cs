using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Tower Defender", menuName = "Tower/Defender")]
public class TowerDefenderConfig : ScriptableObject
{
    public int Index;
    public Sprite MenuSprite;
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
    public PurchaseStats PurchaseStats;

    public DefenderData(TowerDefenderStats stats, PurchaseStats purchaseStats)
    {
        Stats = stats;
        PurchaseStats = purchaseStats;
    }
}