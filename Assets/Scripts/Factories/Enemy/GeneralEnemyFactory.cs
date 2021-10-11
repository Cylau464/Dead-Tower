using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "General Enemy Factory", menuName = "Factories/Enemy/General Enemy Factory")]
public class GeneralEnemyFactory : EnemyFactory
{
    [SerializeField] private EnemyConfig[] _enemies;
    private Dictionary<int, float> _pseudoRandomCounter = new Dictionary<int, float>();

    protected override EnemyConfig GetConfig(int maxPower, LevelDifficulty difficulty)
    {
        if (maxPower <= 0)
            throw new System.Exception("Tried get enemy config with 0 power");

        if (_enemies.Length <= 0)
            throw new System.Exception("Not a single enemy was set on " + this.ToString());

        var enemies = _enemies.Where(x => x.Stats.Power <= maxPower).ToArray();

        if (difficulty.BossLevel == true)
        {
            EnemyConfig config = _enemies[0];
            config.Stats.Power = difficulty.PowerReserve;
            return config;
        }
        else
            return GetRandomConfig(enemies, maxPower + 1);
    }

    private EnemyConfig GetRandomConfig(EnemyConfig[] configs, int maxPower)
    {
        if (_pseudoRandomCounter == null)
            _pseudoRandomCounter = new Dictionary<int, float>();

        int totalPower = 0;

        foreach (EnemyConfig config in configs)
        {
            totalPower += maxPower - config.Stats.Power;

            if (_pseudoRandomCounter.ContainsKey(config.Stats.Power) == false)
                _pseudoRandomCounter.Add(config.Stats.Power, 0f);
        }

        float random = Random.value * totalPower;

        for (int j = 0; j < configs.Length; j++)
        {
            if (random < maxPower - (configs[j].Stats.Power - _pseudoRandomCounter[configs[j].Stats.Power]))
            {
                foreach (KeyValuePair<int, float> kvp in _pseudoRandomCounter.ToArray())
                {
                    if (kvp.Key == configs[j].Stats.Power)
                        _pseudoRandomCounter[kvp.Key] = 0f;
                    else
                        _pseudoRandomCounter[kvp.Key] += kvp.Key * .10f;
                }

                return configs[j];
            }
            else
            {
                random -= maxPower - configs[j].Stats.Power;
                _pseudoRandomCounter[configs[j].Stats.Power] += configs[j].Stats.Power * .0f;
            }
        }

        Debug.LogWarning("Couldn't get random config by weight randomization");

        return configs[Random.Range(0, configs.Length)];
    }
}
