using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower/Tower")]
public class TowerConfig : ScriptableObject
{
    public int Index;
    public Sprite MenuSprite;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public DefensiveWeaponConfig DefensiveWeaponConfig;
    public WeaponConfig WeaponConfig;
    public TowerStats Stats;
    public PurchaseStats PurchaseStats;
}

public class TowerData
{
    public TowerStats Stats;
    public PurchaseStats PurchaseStats;

    public TowerData(TowerStats stats, PurchaseStats purchaseStats)
    {
        Stats = stats;
        PurchaseStats = purchaseStats;
    }
}