using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Tower Defender", menuName = "Tower/Defender")]
public class TowerDefenderConfig : UnitBasicConfig
{
    public Projectile ProjectilePrefab;
    public ModifierConfig Modifier;
    public TowerDefenderStats Stats;
}

public class DefenderData : UnitData
{
    public TowerDefenderStats Stats;

    public DefenderData(int index, TowerDefenderStats stats, bool isPurchased)
    {
        Index = index;
        Stats = stats;
        IsPurchased = isPurchased;
    }
}