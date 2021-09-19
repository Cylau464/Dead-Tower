using UnityEngine;

[CreateAssetMenu(fileName = "Stage Config", menuName = "Levels/Stage Config")]
public class StageConfig : ScriptableObject
{
    public int Index;
    public int LevelsCount;
    public int MinEnemyPower;
    public int MaxEnemyPower;
    public int MinPowerReserve;
    public int MaxPowerReserve;
    public int BossInterval;
}
