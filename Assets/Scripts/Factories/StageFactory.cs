using UnityEngine;

[CreateAssetMenu(fileName = "Stage Factory", menuName = "Factories/Levels/Stage Factory")]
public class StageFactory : GameObjectFactory
{
    [SerializeField] private Level _normalLevel, _bossLevel;

    public Level[] GetStageLevels(StageConfig stageConfig)
    {
        LevelConfig[] levelConfigs;
        bool levelsLoaded;

        if (SLS.Data.Game.Levels.Value.Count > stageConfig.Index
            && SLS.Data.Game.Levels.Value[stageConfig.Index] != null)
        {
            levelConfigs = SLS.Data.Game.Levels.Value[stageConfig.Index];
            levelsLoaded = true;
        }
        else
        {
            levelConfigs = new LevelConfig[stageConfig.LevelsCount];
            levelsLoaded = false;
        }

        Level[] levels = new Level[stageConfig.LevelsCount];

        for(int i = 0; i < levels.Length; i++)
        {
            LevelDifficulty levelDifficulty = new LevelDifficulty();
            Level level = null;
            LevelStatus status = stageConfig.Index == 0 && i == 0 ? LevelStatus.Opened : LevelStatus.Closed;

            if ((i + 1) % stageConfig.BossInterval == 0)
            {
                levelDifficulty.BossLevel = true;
                level = _bossLevel;
            }
            else
            {
                levelDifficulty.BossLevel = false;
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
            levelDifficulty.MaxEnemyPower = maxPower;
            levelDifficulty.PowerReserve = powerReserve;

            if (levelsLoaded == false)
                levelConfigs[i] = new LevelConfig(stageConfig.Index, i, status, levelDifficulty);

            var instance = CreateGameObjectInstance(level);
            instance.Init(levelConfigs[i]);
            levels[i] = instance;
        }

        if(levelsLoaded == false)
        {
            if (SLS.Data.Game.Levels.Value.Count > stageConfig.Index)
                SLS.Data.Game.Levels.Value[stageConfig.Index] = levelConfigs;
            else
                SLS.Data.Game.Levels.Value.Add(levelConfigs);
        }

        return levels;
    }
}
