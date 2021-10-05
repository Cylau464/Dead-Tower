using UnityEngine;
using System.Linq;

public class AssetsHolder : MonoBehaviour
{
    [SerializeField] private ResourceConfig[] _resourceConfigs;
    public ResourceConfig[] ResourceConfigs => _resourceConfigs;
    [SerializeField] private ProjectileConfig[] _projectileConfigs;
    public ProjectileConfig[] ProjectileConfigs => _projectileConfigs;
    [SerializeField] private TowerConfig[] _towerConfigs;
    public TowerConfig[] TowerConfigs => _towerConfigs;
    [SerializeField] private TowerDefenderConfig[] _defenderConfigs;
    public TowerDefenderConfig[] DefenderConfigs => _defenderConfigs;

    public static AssetsHolder Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach(TowerConfig config in _towerConfigs)
        {
            TowerData data = SLS.Data.Game.Towers.Value[config.Index];
            config.Stats = data.Stats;
            config.PurchaseStats = data.PurchaseStats;
        }

        foreach(TowerDefenderConfig config in _defenderConfigs)
        {
            DefenderData data = SLS.Data.Game.Defenders.Value[config.Index];
            config.Stats = data.Stats;
            config.PurchaseStats = data.PurchaseStats;
        }

        _towerConfigs = _towerConfigs.OrderBy(x => x.Index).ToArray();
        _defenderConfigs = _defenderConfigs.OrderBy(x => x.Index).ToArray();
        _projectileConfigs = _projectileConfigs.OrderBy(x => x.Index).ToArray();
    }
}