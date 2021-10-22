using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory _generalFactory;
    [SerializeField] private EnemyFactory _bossFactory;
    private EnemyFactory _factory;
    [Space]
    [SerializeField] private Transform _generalSpawnPoint;
    [SerializeField] private Transform _bossSpawnPoint;
    [Space]
    [SerializeField] private float _respawnCooldown = 3f;
    [Space]
    [SerializeField] private Skull _skull;

    private int _powerReserve;
    private Transform _spawnPoint;

    public static LevelConfig LevelConfig;

    public static Action<Enemy> OnEnemySpawned;

    private void Start()
    {
        _skull.gameObject.SetActive(SkullRandomizer.Instance.SkullEnabled);
        _powerReserve = LevelConfig.Difficulty.PowerReserve;

        if (LevelConfig.Difficulty.BossLevel == true)
        {
            _factory = _bossFactory;
            _spawnPoint = _bossSpawnPoint;
        }
        else
        {
            _factory = _generalFactory;
            _spawnPoint = _generalSpawnPoint;
        }

        StartCoroutine(SpawnEnemy());

        Game.Instance.OnLevelEnd += OnLevelEnd;
    }

    private IEnumerator SpawnEnemy()
    {
        yield return null;

        while(_powerReserve > 0)
        {
            Enemy enemy = _factory.Get(
                Mathf.Min(LevelConfig.Difficulty.MaxEnemyPower, _powerReserve),
                LevelConfig.Difficulty);

            if(SkullRandomizer.Instance.SkullEnabled == false)
            {
                enemy.transform.position = _spawnPoint.position;
                enemy.Spawned();
            }
            else
            {
                _skull.SetEnemy(enemy);
            }

            _powerReserve -= enemy.Stats.Power;
            OnEnemySpawned?.Invoke(enemy);

            yield return new WaitForSeconds(_respawnCooldown);
        }

        Debug.LogWarning("Power reserve depleted!");
    }

    private void OnLevelEnd(bool victory)
    {
        StopAllCoroutines();

        if(victory == true)
        {
            if (LevelConfig.Status != LevelStatus.Completed)
                DailyQuestsHandler.Instance.LevelComplete();

            LevelConfig.Status = LevelStatus.Completed;
            List<LevelConfig[]> levels = SLS.Data.Game.Levels.Value;
            LevelConfig nextLevel = SLS.Data.Game.GetNextLevel();
            nextLevel.Status = nextLevel.Status == LevelStatus.Closed ? LevelStatus.Opened : nextLevel.Status;

        }
    }

    private void OnDestroy()
    {
        Game.Instance.OnLevelEnd -= OnLevelEnd;
    }
}
