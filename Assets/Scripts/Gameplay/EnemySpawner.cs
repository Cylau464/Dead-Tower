using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory _generalFactory = null;
    [SerializeField] private EnemyFactory _bossFactory = null;
    private EnemyFactory _factory;
    [SerializeField] private float _respawnCooldown = 3f;
    private int _powerReserve;

    public static LevelConfig LevelConfig;

    public static Action<Enemy> OnEnemySpawned;

    private void Start()
    {
        _powerReserve = LevelConfig.Difficulty.PowerReserve;

        if (LevelConfig.Difficulty.BossLevel == true)
            _factory = _bossFactory;
        else
            _factory = _generalFactory;

        StartCoroutine(SpawnEnemy());

        Game.Instance.OnLevelEnd += OnLevelEnd;
    }

    private IEnumerator SpawnEnemy()
    {
        while(_powerReserve > 0)
        {
            Enemy enemy = _factory.Get(
                Mathf.Min(LevelConfig.Difficulty.MaxEnemyPower, _powerReserve),
                LevelConfig.Difficulty);
            enemy.transform.position = transform.position;
            _powerReserve -= enemy.Stats.Power;

            OnEnemySpawned?.Invoke(enemy);
            Debug.Log(enemy.name + " was spawned");

            yield return new WaitForSeconds(_respawnCooldown);
        }

        Debug.LogWarning("Power reserve depleted!");
    }

    private void OnLevelEnd(bool victory)
    {
        if(victory == true)
        {
            LevelConfig.Status = LevelStatus.Completed;
            List<LevelConfig[]> levels = SLS.Data.Game.Levels.Value;
            LevelConfig nextLevel = SLS.Data.Game.GetNextLevel();
            nextLevel.Status = nextLevel.Status == LevelStatus.Closed ? LevelStatus.Opened : nextLevel.Status;

            StopAllCoroutines();
        }
    }

    private void OnDestroy()
    {
        Game.Instance.OnLevelEnd -= OnLevelEnd;
    }
}
