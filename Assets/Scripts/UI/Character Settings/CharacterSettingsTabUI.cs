using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class CharacterSettingsTabUI : TabUI
{
    [Space]
    [SerializeField] private UnitBasicConfig[] _itemsConfigs;
    [SerializeField] private CharacterSettingsFactory _factory;

    private CharacterSettingsUI _windowUI;
    private Button _prevItemBtn;
    private Button _nextItemBtn;
    private Button _selectBtn;
    private TextMeshProUGUI _itemTitle;
    private TextMeshProUGUI _description;

    private UnitBasicConfig _curConfig;

    public Action OnSelected;

    private void Activate()
    {
        if (_isInitialized == false) return;

        _prevItemBtn.onClick.AddListener(PrevItem);
        _nextItemBtn.onClick.AddListener(NextItem);
        _selectBtn.onClick.AddListener(Select);
        SwitchItem(_curConfig.Index);
    }

    private void Deactivate()
    {
        if (_isInitialized == false) return;

        _prevItemBtn.onClick.RemoveListener(PrevItem);
        _nextItemBtn.onClick.RemoveListener(NextItem);
        _selectBtn.onClick.RemoveListener(Select);
    }

    private void SetItemTitle()
    {
        _itemTitle.text = _curConfig.Title;
        _description.text = _curConfig.Description;
    }

    public void Init(ToggleGroup group, CharacterSettingsUI windowUI)
    {
        base.Init(group);

        foreach (UnitBasicConfig config in _itemsConfigs)
        {
            CSItem item = _factory.GetItem(config, this);
            item.transform.SetParent(_contentHolder, false);
        }

        _windowUI = windowUI;
        _prevItemBtn = windowUI.PrevItemBtn;
        _nextItemBtn = windowUI.NextItemBtn;
        _selectBtn = windowUI.SelectBtn;
        _itemTitle = windowUI.TitleText;
        _description = windowUI.DescriptionText;
        _scrollRect.transform.position = windowUI.ScrollView.position;

        if (_itemsConfigs[0] is TowerConfig)
        {
            SLS.Data.Game.Towers.OnValueChanged += OnItemsChanged;
            SwitchItem(_itemsConfigs.First(x => x.Index == SLS.Data.Game.SelectedTower.Value.Index).Index);
        }
        else
        {
            SLS.Data.Game.Defenders.OnValueChanged += OnItemsChanged;
            SwitchItem(_itemsConfigs.First(x => x.Index == SLS.Data.Game.SelectedDefender.Value.Index).Index);
        }
    }

    protected override void ToggleTab(bool active)
    {
        base.ToggleTab(active);

        if (active == true)
            Activate();
        else
            Deactivate();
    }

    protected override void SortAscendingConfigs()
    {
        _itemsConfigs = _itemsConfigs.OrderBy(x => x.Index).ToArray();
    }

    private void PrevItem()
    {
        if (_curConfig.Index <= 0) return;

        SwitchItem(_curConfig.Index - 1);
    }

    private void NextItem()
    {
        if (_curConfig.Index >= _itemsConfigs.Length - 1) return;

        SwitchItem(_curConfig.Index + 1);
    }

    private void OnItemsChanged(UnitData[] data)
    {
        if(_isInitialized == true)
            SwitchItem(_curConfig.Index);
    }

    private void SwitchItem(int index)
    {
        _curConfig = _itemsConfigs[index];
        SetItemTitle();

        if (_itemsConfigs[0] is TowerConfig)
        {
            _selectBtn.interactable = SLS.Data.Game.Towers.Value[index].IsPurchased == true
                && (SLS.Data.Game.SelectedTower.Value.Index == index) == false;
        }
        else
        {
            _selectBtn.interactable = SLS.Data.Game.Defenders.Value[index].IsPurchased == true
                && (SLS.Data.Game.SelectedDefender.Value.Index == index) == false;
        }

        ScrollTo(index);
    }

    public override void ScrollTo(int itemIndex)
    {
        float offset = 1f / (_itemsConfigs.Length - 1) * itemIndex;

        ScrollTo(_scrollRect, offset);
    }

    private void Select()
    {
        _selectBtn.interactable = false;

        if (_curConfig is TowerConfig)
            SLS.Data.Game.SelectedTower.Value = SLS.Data.Game.Towers.Value[_curConfig.Index];
        else
            SLS.Data.Game.SelectedDefender.Value = SLS.Data.Game.Defenders.Value[_curConfig.Index];

        OnSelected?.Invoke();
    }

    public void OpenShop(int itemIndex)
    {
        _windowUI.Hide();
        MenuSwitcher.Instance.OpenShop(_itemCategory, itemIndex);
    }

    private void OnDestroy()
    {
        if (_itemsConfigs[0] is TowerConfig)
            SLS.Data.Game.Towers.OnValueChanged -= OnItemsChanged;
        else
            SLS.Data.Game.Defenders.OnValueChanged -= OnItemsChanged;
    }
}