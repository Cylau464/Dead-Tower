using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] private CanvasGroupUI _startUI;
    [SerializeField] private CanvasGroupUI _settingsUI;
    [SerializeField] private CanvasGroupUI _mapUI;
    [SerializeField] private CanvasGroupUI _shopUI;
    [SerializeField] private CanvasGroupUI _forgeUI;
    [SerializeField] private CanvasGroupUI _characterSettingsUI;
    [SerializeField] private CanvasGroupUI _projectileShortageUI;
    [SerializeField] private CanvasGroupUI _dailyQuestsUI;

    public static MenuSwitcher Instance;
    public static bool OpenMapAfterLoad;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        (_settingsUI as SettingsUI).Init();

        if (OpenMapAfterLoad == true)
            OpenMap();
        else
            OpenStartMenu();
    }

    public void OpenStartMenu()
    {
        _startUI.Show();
    }

    public void OpenSettings()
    {
        _settingsUI.Show();
    }

    public void OpenMap()
    {
        _mapUI.Show();
    }

    public void OpenShop()
    {
        _shopUI.Show();
    }

    public void OpenShop(ItemCategory category, int itemIndex = 0)
    {
        (_shopUI as ShopUI).Show(category, itemIndex);
    }
    
    public void OpenForge()
    {
        _forgeUI.Show();
    } 
    
    public void OpenCharacterSettings()
    {
        _characterSettingsUI.Show();
    }

    public void OpenProjectileShortage()
    {
        _projectileShortageUI.Show();
    }

    public void HideMap()
    {
        _mapUI.Hide();
    }

    public void OpenDailyQuests()
    {
        _dailyQuestsUI.Show();
    }
}
