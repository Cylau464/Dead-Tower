using UnityEngine;

public class TowerInitializator : MonoBehaviour
{
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private Transform _towerPoint;
    [SerializeField] private TowerDefender _defenderPrefab;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private TowerConfig _towerConfig;
    [SerializeField] private TowerDefenderConfig _defenderConfig;
    [Space]
    [SerializeField] private bool _inMenuInit;

    private void Start()
    {
        Tower tower = Instantiate(_towerPrefab, _towerPoint.position, Quaternion.identity);
        tower.Init(_towerConfig, _inMenuInit);
        TowerDefender defender = Instantiate(_defenderPrefab, tower.DefenderPoint.position, Quaternion.identity, tower.transform);
        defender.Init(_defenderConfig, _inMenuInit);
        
        if(_gameplayUI != null)
            _gameplayUI.SetTower(tower);
    }
}