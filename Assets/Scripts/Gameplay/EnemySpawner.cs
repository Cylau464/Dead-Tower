using System.Collections;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory _generalFactory = null;
    [SerializeField] private EnemyFactory _bossFactory = null;
    private EnemyFactory _factory;
    [SerializeField] private float _respawnCooldown = 3f;

    public static LevelInfo LevelInfo;
    public static LevelConfig LevelConfig;

    public static Action<Enemy> OnEnemySpawned;

    private void Start()
    {
        if (LevelInfo.BossLevel == true)
            _factory = _bossFactory;
        else
            _factory = _generalFactory;

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while(LevelInfo.PowerReserve > 0)
        {
            Enemy enemy = _factory.Get(Mathf.Min(LevelInfo.MaxEnemyPower, LevelInfo.PowerReserve));
            enemy.transform.position = transform.position;
            LevelInfo.PowerReserve -= enemy.Stats.Power;
            OnEnemySpawned?.Invoke(enemy);
            Debug.Log(enemy.name + " was spawned");

            yield return new WaitForSeconds(_respawnCooldown);
        }

        Debug.LogWarning("Power reserve depleted!");
    }
}
