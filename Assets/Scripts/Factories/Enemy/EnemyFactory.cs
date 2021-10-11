using UnityEngine;

public abstract class EnemyFactory : GameObjectFactory
{
    public Enemy Get(int maxPower, LevelDifficulty difficulty)
    {
        var config = GetConfig(maxPower, difficulty);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.Init(config);

        return instance;
    }

    protected abstract EnemyConfig GetConfig(int maxPower, LevelDifficulty difficulty);
}
