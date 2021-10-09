using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CSItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] protected TextMeshProUGUI _statusText;
    [SerializeField] private Button _purchaseBtn;

    private UnitBasicConfig _config;
    private CharacterSettingsTabUI _tab;

    public void Init(UnitBasicConfig config, CharacterSettingsTabUI tab)
    {
        _config = config;
        _tab = tab;
        _tab.OnSelected += OnSelected;
        _icon.sprite = config.MenuIcon;
        _purchaseBtn.onClick.AddListener(BuyItem);
        SetStatus(false);

        if (_config is TowerConfig)
            SLS.Data.Game.Towers.OnValueChanged += OnItemsChanged;
        else
            SLS.Data.Game.Defenders.OnValueChanged += OnItemsChanged;
    }

    private void SetStatus(bool lerpAlpha = true)
    {
        bool isPurchased;
        bool isSelected;

        if (_config is TowerConfig)
        {
            isPurchased = SLS.Data.Game.Towers.Value[_config.Index].IsPurchased;
            isSelected = SLS.Data.Game.SelectedTower.Value.Index == _config.Index;
        }
        else
        {
            isPurchased = SLS.Data.Game.Defenders.Value[_config.Index].IsPurchased;
            isSelected = SLS.Data.Game.SelectedDefender.Value.Index == _config.Index;
        }

        if (isPurchased == true)
        {
            _statusText.text = "Selected";
            _statusText.gameObject.SetActive(isSelected);
        }
        else
        {
            _statusText.text = "Buy Now";
            _statusText.gameObject.SetActive(true);
        }

        _purchaseBtn.interactable = !isPurchased;

        if (lerpAlpha == false || gameObject.activeInHierarchy == false) return;

        Color color = _statusText.color;
        color.a = 0f;
        _statusText.color = color;

        this.ChangeAlpha(
            time: .2f,
            _statusText,
            to: 1f
        );
    }

    private void OnSelected()
    {
        SetStatus();
    }

    private void BuyItem()
    {
        _tab.OpenShop(_config.Index);
    }

    private void OnItemsChanged(UnitData[] data)
    {
        SetStatus();
    }

    private void OnDestroy()
    {
        _tab.OnSelected -= OnSelected;

        if (_config is TowerConfig)
            SLS.Data.Game.Towers.OnValueChanged -= OnItemsChanged;
        else
            SLS.Data.Game.Defenders.OnValueChanged -= OnItemsChanged;
    }
}