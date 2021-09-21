using UnityEngine;

public class LevelInitializator : MonoBehaviour
{
    [SerializeField] private Transform _towerPoint;
    [SerializeField] private TowerDefender _defenderPrefab;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private TowerConfig _towerConfig;
    [SerializeField] private TowerDefenderConfig _defenderConfig;

    private void Start()
    {
        Tower tower = Instantiate(_towerPrefab, _towerPoint.position, Quaternion.identity);
        tower.Init(_towerConfig);
        TowerDefender defender = Instantiate(_defenderPrefab, tower.DefenderPoint.position, Quaternion.identity, tower.transform);
        defender.Init(_defenderConfig);
    }
}