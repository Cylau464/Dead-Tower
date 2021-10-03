using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Image _projectileIcon;
    [SerializeField] private TextMeshProUGUI _projectileCountText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _towerHealthText;

    private int _projectileIndex;
    private Tower _tower;
    private int _startLevelPower;
    private int _levelProgress;

    private void Start()
    {
        _pauseBtn.onClick.AddListener(Pause);
        ProjectileConfig projectileConfig = AssetsHolder.Instance.SelectedTower.WeaponConfig.ProjectileConfig;
        _projectileIndex = projectileConfig.Index;
        _projectileIcon.sprite = projectileConfig.Icon;
        _projectileCountText.text = SLS.Data.Game.ProjectilesCount
            .Value[_projectileIndex].ToString();
        _progressText.text = "0%";
        _towerHealthText.text = AssetsHolder.Instance.SelectedTower.Stats.Health.ToString();

        SLS.Data.Game.ProjectilesCount.OnValueChanged += UpdateProjectileCount;
        EnemySpawner.OnEnemySpawned += OnEnemySpawned;
        _startLevelPower = EnemySpawner.LevelInfo.PowerReserve;
    }

    public void SetTower(Tower tower)
    {
        _tower = tower;
        _tower.OnTakeDamage += OnTowerTakeDamage;
    }

    public void OnEnemySpawned(Enemy enemy)
    {
        enemy.OnDead += OnEnemyDied;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }

    private void UpdateProjectileCount(int[] value)
    {
        Debug.Log(value.Length);
        _projectileCountText.text = value[_projectileIndex].ToString();
    }

    private void OnEnemyDied(Rewards rewards, int value)
    {
        _levelProgress += value;
        int progress = Mathf.Min(Mathf.CeilToInt((float)_levelProgress / _startLevelPower * 100), 100);
        _progressText.text = progress.ToString() + "%";
    }

    private void OnTowerTakeDamage(int value)
    {
        _towerHealthText.text = value.ToString();
    }

    private void OnDestroy()
    {
        SLS.Data.Game.ProjectilesCount.OnValueChanged -= UpdateProjectileCount;
        _tower.OnTakeDamage -= OnTowerTakeDamage;
        EnemySpawner.OnEnemySpawned -= OnEnemySpawned;
    }
}
