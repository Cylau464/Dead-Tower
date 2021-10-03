using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Tower Defender", menuName = "Tower/Defender")]
public class TowerDefenderConfig : ScriptableObject
{
    public int Index;
    public Sprite MenuSprite;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public Projectile ProjectilePrefab;
    public ModifierConfig Modifier;
    public TowerDefenderStats Stats;
    public PurchaseStats PurchaseStats;
}

public class DefenderData
{
    public TowerDefenderStats Stats;
    public PurchaseStats PurchaseStats;

    public DefenderData(TowerDefenderStats stats, PurchaseStats purchaseStats)
    {
        Stats = stats;
        PurchaseStats = purchaseStats;
    }
}