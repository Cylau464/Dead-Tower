using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Button PrevItemBtn => _prevItemBtn;
    public Button NextItemBtn => _nextItemBtn;
    public Button SelectBtn => _selectBtn;
    public TextMeshProUGUI TitleText => _titleText;
    public TextMeshProUGUI DescriptionText => _descriptionText;
    public RectTransform ScrollView => _scrollView;

    private void Start()
    {
        for (int i = 0; i < _tabs.Length; i++)
            _tabs[i].Init(_toggleGroup, this);

        _toggleGroup.SetAllTogglesOff();

        _closeBtn.onClick.AddListener(Close);
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }

    public void OpenItemInShop(UnitBasicConfig itemConfig)
    {
        Hide();

    }
}
