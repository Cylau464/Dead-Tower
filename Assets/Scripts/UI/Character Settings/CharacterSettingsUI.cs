using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharacterSettingsUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private RectTransform _scrollView;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private CharacterSettingsTabUI[] _tabs;
    [Space]
    [SerializeField] private Button _prevItemBtn;
    [SerializeField] private Button _nextItemBtn;
    [SerializeField] private Button _selectBtn;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Space]
    [SerializeField] private StatsWindowUI _statsWindowUI;

    public Button PrevItemBtn => _prevItemBtn;
    public Button NextItemBtn => _nextItemBtn;
    public Button SelectBtn => _selectBtn;
    public TextMeshProUGUI TitleText => _titleText;
    public TextMeshProUGUI DescriptionText => _descriptionText;
    public RectTransform ScrollView => _scrollView;

    private void Start()
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            _tabs[i].Init(_toggleGroup, this);
            _tabs[i].OnSwitchItem += OnSwitchItem;
        }

        _toggleGroup.SetAllTogglesOff();

        _closeBtn.onClick.AddListener(Close);
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }

    private void OnSwitchItem(object sender, UnitBasicConfig config)
    {
        CharacterSettingsTabUI tab = _tabs.First(x => (object)x == sender);
        UnitData data;

        if (config is TowerConfig)
            data = SLS.Data.Game.Towers.Value[config.Index];
        else
            data = SLS.Data.Game.Defenders.Value[config.Index];

        _statsWindowUI.SwitchItem(data);
    }

    private void OnDestroy()
    {
        foreach(CharacterSettingsTabUI tab in _tabs)
            tab.OnSwitchItem -= OnSwitchItem;
    }
}
