using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ProjectilesShortageUI : CanvasGroupUI
{
    [Space]
    [TextArea] [SerializeField] private string _title;
    [SerializeField] private Image _projectileIcon;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _titleText;
    [Space]
    [SerializeField] private Button _closeBtn;
    [Header("Menu buttons")]
    [SerializeField] private Button _forgeBtn;
    [SerializeField] private Button _characterSettingsBtn;
    [Header("Gameplay buttons")]
    [SerializeField] private Button _diamondsPayBtn;
    [SerializeField] private Button _viewAdBtn;
    [SerializeField] private TMP_Text _diamondsPayText;
    [Space]
    [SerializeField] private int _projectileCount;
    [SerializeField] private int _diamondsPayCost;
    [Space]
    [SerializeField] private bool _inMenu = true;

    private void Start()
    {
        int index = SLS.Data.Game.SelectedTower.Value.Index;
        _projectileIcon.sprite = AssetsHolder.Instance.TowerConfigs[index].WeaponConfig.ProjectileConfig.Icon;
        _countText.text = SLS.Data.Game.ProjectilesCount.Value[index].ToString();
        _titleText.text = _title;
        _closeBtn.onClick.AddListener(Close);

        if(_inMenu == true)
        {
            _forgeBtn.onClick.AddListener(OpenForge);
            _characterSettingsBtn.onClick.AddListener(OpenCharactrerSettings);

            _diamondsPayBtn?.gameObject.SetActive(false);
            _viewAdBtn?.gameObject.SetActive(false);
        }
        else
        {
            _diamondsPayBtn.onClick.AddListener(DiamondsPay); 
            _viewAdBtn.onClick.AddListener(ViewAd);
            _diamondsPayBtn.interactable = SLS.Data.Game.Diamonds.Value >= _diamondsPayCost;
            _diamondsPayText.text = _diamondsPayCost.ToString();
            _viewAdBtn.interactable = AdsInitializer.Instance.RewardedAd.IsLoaded;
            AdsInitializer.Instance.RewardedAd.OnAdComplete += OnAdComplete;
            AdsInitializer.Instance.RewardedAd.OnAdLoaded += OnAdLoaded;

            _forgeBtn.gameObject.SetActive(false);
            _characterSettingsBtn.gameObject.SetActive(false);
        }
    }

    public override void Show()
    {
        base.Show();

        this.DoAfterNextFrameCoroutine(() => _closeBtn.interactable = true);

        if (_inMenu == false)
            Time.timeScale = 0f;
    }

    public override void Hide()
    {
        base.Hide();
        _closeBtn.interactable = false;

        if (_inMenu == false)
            Time.timeScale = 1f;
    }

    private void Close()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    private void OpenForge()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.HideMap();
        MenuSwitcher.Instance.OpenForge();
    }

    private void OpenCharactrerSettings()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.HideMap();
        MenuSwitcher.Instance.OpenCharacterSettings();
    }

    private void DiamondsPay()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        SLS.Data.Game.Diamonds.Value -= _diamondsPayCost;
        int[] projectilesCount = SLS.Data.Game.ProjectilesCount.Value.ToArray();
        projectilesCount[SLS.Data.Game.SelectedTower.Value.Index] += _projectileCount;
        SLS.Data.Game.ProjectilesCount.Value = projectilesCount;
        Hide();
    }

    private void ViewAd()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _viewAdBtn.interactable = false;
        AdsInitializer.Instance.ShowRewarded();
    }

    private void OnAdComplete()
    {
        Game.Instance.LevelEnd(true, true);
        Hide();
    }

    private void OnAdLoaded()
    {
        _viewAdBtn.interactable = true;
    }

    private void OnDestroy()
    {
        AdsInitializer.Instance.RewardedAd.OnAdComplete -= OnAdComplete;
        AdsInitializer.Instance.RewardedAd.OnAdLoaded -= OnAdLoaded;
    }
}