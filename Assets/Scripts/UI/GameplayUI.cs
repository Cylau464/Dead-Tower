using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private WinUI _winUI;
    [SerializeField] private CanvasGroupUI _loseUI;
    [SerializeField] private CanvasGroupUI _pauseUI;
    [SerializeField] private CanvasGroupUI _projectileShortageUI;
    [Space]
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Image _projectileIcon;
    [SerializeField] private TextMeshProUGUI _projectileCountText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _towerHealthText;
    [Space]
    [SerializeField] private float _levelEndDelay = 2f;

    private Rewards _rewards;
    private int _projectileIndex;
    private Tower _tower;
    private int _startLevelPower;
    private int _levelProgress;

    private void Start()
    {
        _pauseBtn.onClick.AddListener(Pause);
        _progressText.text = "0%";

        SLS.Data.Game.ProjectilesCount.OnValueChanged += UpdateProjectileCount;
        EnemySpawner.OnEnemySpawned += OnEnemySpawned;
        _startLevelPower = EnemySpawner.LevelConfig.Difficulty.PowerReserve;
        Game.Instance.OnLevelEnd += OnLevelEnd;
    }

    public void SetTower(Tower tower)
    {
        _tower = tower;
        _tower.OnTakeDamage += OnTowerTakeDamage;
        ProjectileConfig projectileConfig = _tower.Weapon.Config.ProjectileConfig;
        _projectileIndex = projectileConfig.Index;
        _projectileIcon.sprite = projectileConfig.Icon;
        _projectileCountText.text = SLS.Data.Game.ProjectilesCount
            .Value[_projectileIndex].ToString();
        _towerHealthText.text = _tower.Stats.Health.ToString();
        _tower.Weapon.OnProjectilesEnd += OpenProjectileShortage;
    }

    public void OnEnemySpawned(Enemy enemy)
    {
        enemy.OnDead += OnEnemyDied;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        _pauseUI.Show();
    }

    private void UpdateProjectileCount(int[] value)
    {
        _projectileCountText.text = value[_projectileIndex].ToString();
    }

    private void OnEnemyDied(Rewards rewards, int value)
    {
        _levelProgress += value;
        int progress = Mathf.Min(Mathf.CeilToInt((float)_levelProgress / _startLevelPower * 100), 100);
        _rewards += rewards;
        this.LerpCoroutine(
            time: .125f,
            from: Vector3.one,
            to: Vector3.one * 1.5f,
            action: a => _progressText.transform.localScale = a,
            settings: new CoroutineTemplate.Settings(false, 0, true)
        );
        this.LerpCoroutine(
            time: .25f,
            from: int.Parse(Regex.Match(_progressText.text, @"\d+").Value),
            to: progress,
            action: a => _progressText.text = Mathf.Round(a).ToString() + "%"
        );

        if (progress >= 100)
            Game.Instance.LevelEnd(true);
    }

    private void OnLevelEnd(bool victory)
    {
        StartCoroutine(LevelEndDelay(victory));
    }

    private IEnumerator LevelEndDelay(bool victory)
    {
        _winUI.Hide();
        _loseUI.Hide();

        yield return new WaitForSeconds(_levelEndDelay);

        if (victory == true)
            _winUI.Show(_rewards);
        else
            _loseUI.Show();

        AdsInitializer.Instance.ShowInterstitial();
    }

    private void OnTowerTakeDamage(int value)
    {
        _towerHealthText.text = value.ToString();
        this.LerpCoroutine(
            time: .125f,
            from: Vector3.one,
            to: Vector3.one * 1.5f,
            action: a => _towerHealthText.transform.localScale = a,
            settings: new CoroutineTemplate.Settings(false, 0, true)
        );
    }

    private void OpenProjectileShortage()
    {
        _projectileShortageUI.Show();
    }

    private void OnDestroy()
    {
        SLS.Data.Game.ProjectilesCount.OnValueChanged -= UpdateProjectileCount;
        _tower.OnTakeDamage -= OnTowerTakeDamage;
        EnemySpawner.OnEnemySpawned -= OnEnemySpawned;
        Game.Instance.OnLevelEnd -= OnLevelEnd;
        _tower.Weapon.OnProjectilesEnd -= OpenProjectileShortage;
    }
}
