using UnityEngine;
using UnityEngine.UI;

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

        base.Init();
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

    }

    private void OpenCharacterSettings()
    {

    }

    private void OpenCurrencyShop()
    {

    }
}