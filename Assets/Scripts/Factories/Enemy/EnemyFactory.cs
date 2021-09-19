using UnityEngine;

public abstract class EnemyFactory : GameObjectFactory
{
    public Enemy Get(int maxPower)
    {
        var config = GetConfig(maxPower);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.Init(config);

        return instance;
    }

    protected abstract EnemyConfig GetConfig(int maxPower);
}
