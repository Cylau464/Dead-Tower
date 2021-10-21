using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _forgeBtn;
    [SerializeField] private Button _shopBtn;
    [SerializeField] private Button _characterBtn;
    [SerializeField] private Button _dailyQuestsBtn;
    [SerializeField] private Button _noAdsBtn;
    [Space]
    [SerializeField] private Button _projectilesBtn;
    [SerializeField] private Image _projectilesImage;
    [SerializeField] private TextMeshProUGUI _projectileCountText;
    [Space]
    [SerializeField] private Button _buyCurrencyBtn;
    [Space]
    [SerializeField] private CanvasGroupUI _noAdsUI;

    protected override void Init()
    {
        _backBtn.onClick.AddListener(BackToMenu);

        _forgeBtn.onClick.AddListener(OpenForge);
        _shopBtn.onClick.AddListener(OpenShop);
        _characterBtn.onClick.AddListener(OpenCharacterSettings);
        _dailyQuestsBtn.onClick.AddListener(OpenDailyQuests);
        _noAdsBtn.onClick.AddListener(OpenNoAds);

        _projectilesBtn.onClick.AddListener(OpenForge);

        _buyCurrencyBtn.onClick.AddListener(OpenCurrencyShop);

        SLS.Data.Game.ProjectilesCount.OnValueChanged += UpdateProjectilesCount;

        base.Init();
    }

    public override void Show()
    {
        base.Show();

        _noAdsBtn.gameObject.SetActive(SLS.Data.Settings.AdsEnabled.Value);
        UpdateProjectilesCount(SLS.Data.Game.ProjectilesCount.Value);
    }

    private void BackToMenu()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenStartMenu();
    }

    private void OpenForge()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenForge();
    }

    private void OpenShop()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenShop();
    }

    private void OpenCharacterSettings()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenCharacterSettings();
    }

    private void OpenCurrencyShop()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenShop(ItemCategory.Currency);
    }

    private void OpenDailyQuests()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenDailyQuests();
    }

    private void OpenNoAds()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _noAdsUI.Show();
    }

    private void UpdateProjectilesCount(int[] projectiles)
    {
        int index = AssetsHolder.Instance.TowerConfigs[SLS.Data.Game.SelectedTower.Value.Index]
            .WeaponConfig.ProjectileConfig.Index;

        _projectilesImage.sprite = AssetsHolder.Instance.ProjectileConfigs[index].MapIcon;
        _projectileCountText.text = projectiles[index].ToString();
    }

    private void OnDestroy()
    {
        SLS.Data.Game.ProjectilesCount.OnValueChanged -= UpdateProjectilesCount;
    }
}