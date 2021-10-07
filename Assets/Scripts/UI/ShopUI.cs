using UnityEngine;
using UnityEngine.UI;

public class ShopUI : CanvasGroupUI
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TabUI[] _tabs;
    [SerializeField] private ToggleGroup _toggleGroup;

    private void Start()
    {
        for(int i = 0; i < _tabs.Length; i++)
            _tabs[i].Init(_toggleGroup);

        _toggleGroup.SetAllTogglesOff();

        _closeBtn.onClick.AddListener(Close);
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }
}
