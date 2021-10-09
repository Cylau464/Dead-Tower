using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private StartMenuUI _startMenu;
    [SerializeField] private ForgeUI _forgeUI;
    [Space]
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _forgeBtn;
    [SerializeField] private Button _shopBtn;
    [SerializeField] private Button _characterBtn;
    [Space]
    [SerializeField] private Button _projectilesBtn;
    [SerializeField] private TextMeshProUGUI _projectileCountText;
    [Space]
    [SerializeField] private Button _buyCurrencyBtn;

    protected override void Init()
    {
        _backBtn.onClick.AddListener(BackToMenu);

        _forgeBtn.onClick.AddListener(OpenForge);
        _shopBtn.onClick.AddListener(OpenShop);
        _characterBtn.onClick.AddListener(OpenCharacterSettings);

        _projectilesBtn.onClick.AddListener(OpenForge);

        _buyCurrencyBtn.onClick.AddListener(OpenCurrencyShop);

        SLS.Data.Game.ProjectilesCount.OnValueChanged += UpdateProjectilesCount;

        base.Init();
    }

    public override void Show()
    {
        base.Show();

        UpdateProjectilesCount(SLS.Data.Game.ProjectilesCount.Value);
    }

    private void BackToMenu()
    {
        Hide();
        MenuSwitcher.Instance.OpenStartMenu();
    }

    private void OpenForge()
    {
        Hide();
        MenuSwitcher.Instance.OpenForge();
    }

    private void OpenShop()
    {
        Hide();
        MenuSwitcher.Instance.OpenShop();
    }

    private void OpenCharacterSettings()
    {
        Hide();
        MenuSwitcher.Instance.OpenCharacterSettings();
    }

    private void OpenCurrencyShop()
    {
        Hide();
        MenuSwitcher.Instance.OpenShop(ItemCategory.Currency);
    }

    private void UpdateProjectilesCount(int[] projectiles)
    {
        int index = AssetsHolder.Instance.TowerConfigs[SLS.Data.Game.SelectedTower.Value.Index]
            .WeaponConfig.ProjectileConfig.Index;
        _projectileCountText.text = projectiles[index].ToString();
    }

    private void OnDestroy()
    {
        SLS.Data.Game.ProjectilesCount.OnValueChanged -= UpdateProjectilesCount;
    }
}