using UnityEngine;

[CreateAssetMenu(fileName = "Stage Config", menuName = "Levels/Stage Config")]
public class StageConfig : ScriptableObject
{
    public LineRenderer SpawnCurve;
    public Sprite Sprite;
    public int Index;
    [Space]
    public int LevelsCount;
    [Space]
    public int MinEnemyPower;
    public int MaxEnemyPower;
    [Space]
    public int MinPowerReserve;
    public int MaxPowerReserve;
    public int BossInterval;
}
