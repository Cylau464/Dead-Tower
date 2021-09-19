using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "Stage Factory", menuName = "Factories/Levels/Stage Factory")]
public class StageFactory : GameObjectFactory
{
    [SerializeField] private Level _normalLevel, _bossLevel;

    public Level[] GetStageLevels(StageConfig stageConfig)
    {
        Level[] levels = new Level[stageConfig.LevelsCount];

        for(int i = 0; i < levels.Length; i++)
        {
            LevelInfo levelInfo = new LevelInfo();
            Level level = null;
            LevelStatus status = i == 0 ? LevelStatus.Opened : LevelStatus.Closed;
            LevelConfig levelConfig = new LevelConfig(stageConfig.Index, i, status);

            if ((i + 1) / stageConfig.BossInterval == 1)
            {
                levelInfo.BossLevel = true;
                level = _bossLevel;
            }
            else
            {
                levelInfo.BossLevel = false;
                level = _normalLevel;
            }

            float stagePercent = (float)i / stageConfig.LevelsCount;
            int maxPower = Mathf.Max(
                stageConfig.MinEnemyPower + Mathf.CeilToInt(stagePercent * (stageConfig.MaxEnemyPower - stageConfig.MinEnemyPower)),
                stageConfig.MinEnemyPower
                );
            maxPower = Mathf.Min(maxPower, stageConfig.MaxEnemyPower);
            int powerReserve = Mathf.Max(
                stageConfig.MinPowerReserve + Mathf.CeilToInt(stagePercent * (stageConfig.MaxPowerReserve - stageConfig.MinPowerReserve)),
                stageConfig.MinPowerReserve
                );
            powerReserve = Mathf.Min(powerReserve, stageConfig.MaxPowerReserve);
            levelInfo.MaxEnemyPower = maxPower;
            levelInfo.PowerReserve = powerReserve;

            var instance = CreateGameObjectInstance(level);
            instance.Init(levelConfig, levelInfo);
            levels[i] = instance;
        }

        return levels;
    }
}
