using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private Cloud _cloudPrefab;
    [SerializeField] private int _cloudsCount = 10;
    [SerializeField] private Vector2 _minSpawnPos;
    [SerializeField] private Vector2 _maxSpawnPos;
    [SerializeField] private float _spawnInterval = .5f;

    private Stack<Cloud> _cloudsPool;

    private void Start()
    {
        _cloudsPool = new Stack<Cloud>(_cloudsCount);
        Transform cloudsHolder = new GameObject("Clouds Pool").transform;

        for(int i = 0; i < _cloudsCount; i++)
        {
            Vector2 spawnPos = Vector2.Lerp(_minSpawnPos, _maxSpawnPos, Random.value);
            Cloud cloud = Instantiate(_cloudPrefab, spawnPos, Quaternion.identity, cloudsHolder);
            cloud.Init(this);
            cloud.gameObject.SetActive(false);
            _cloudsPool.Push(cloud);
        }

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if(_cloudsPool.Count > 0)
            {
                Cloud cloud = _cloudsPool.Pop();
                cloud.gameObject.SetActive(true);
                cloud.transform.position = Vector2.Lerp(_minSpawnPos, _maxSpawnPos, Random.value);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public void ReturnToPool(Cloud cloud)
    {
        _cloudsPool.Push(cloud);
    }
}
