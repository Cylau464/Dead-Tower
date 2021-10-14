using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopUI : CanvasGroupUI
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TabUI[] _tabs;
    [SerializeField] private ToggleGroup _toggleGroup;

    protected override void Init()
    {
        for(int i = 0; i < _tabs.Length; i++)
            _tabs[i].Init(_toggleGroup);

        _toggleGroup.SetAllTogglesOff();

        _closeBtn.onClick.AddListener(Close);

        base.Init();
    }

    public void Show(ItemCategory category, int itemIndex = -1)
    {
        base.Show();
        ShopTabUI tab = _tabs.Cast<ShopTabUI>().FirstOrDefault(x => x.ItemCategory == category);
        tab.ActivateTab();

        if(itemIndex >= 0)
            tab.ScrollTo(itemIndex);
    }

    private void Close()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenMap();
    }
}
