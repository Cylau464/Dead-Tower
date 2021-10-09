using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower/Tower")]
public class TowerConfig : UnitBasicConfig
{
    public DefensiveWeaponConfig DefensiveWeaponConfig;
    public WeaponConfig WeaponConfig;
    public TowerStats Stats;
}

public class TowerData : UnitData
{
    public TowerStats Stats;

    public TowerData(int index, TowerStats stats, bool isPurchased)
    {
        Index = index;
        Stats = stats;
        IsPurchased = isPurchased;
    }
}

public class UnitData
{
    public int Index;
    public bool IsPurchased;
}