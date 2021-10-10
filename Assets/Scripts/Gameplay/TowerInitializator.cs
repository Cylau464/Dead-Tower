using UnityEngine;

public class TowerInitializator : MonoBehaviour
{
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private Transform _towerPoint;
    [SerializeField] private TowerDefender _defenderPrefab;
    [SerializeField] private Tower _towerPrefab;
    [Space]
    [SerializeField] private bool _inMenuInit;

    private void Start()
    {
        TowerConfig towerConfig = AssetsHolder.Instance.TowerConfigs[SLS.Data.Game.SelectedTower.Value.Index];
        Tower tower = Instantiate(_towerPrefab, _towerPoint.position, Quaternion.identity);
        tower.Init(towerConfig, SLS.Data.Game.SelectedTower.Value, _inMenuInit);
        TowerDefenderConfig defenderConfig = AssetsHolder.Instance.DefenderConfigs[SLS.Data.Game.SelectedDefender.Value.Index];
        TowerDefender defender = Instantiate(_defenderPrefab, tower.DefenderPoint.position, Quaternion.identity, tower.transform);
        defender.Init(defenderConfig, SLS.Data.Game.SelectedDefender.Value, _inMenuInit);
        
        if(_gameplayUI != null)
            _gameplayUI.SetTower(tower);
    }
}