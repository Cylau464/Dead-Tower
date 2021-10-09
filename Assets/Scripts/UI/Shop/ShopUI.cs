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

    public void Show(ItemCategory category, int itemIndex)
    {
        base.Show();

        switch(category)
        {
            case ItemCategory.Tower:
                break;
            case ItemCategory.Defender:
                break;
            case ItemCategory.Resources:
                break;
            case ItemCategory.Currency:
                _tabs.Cast<ShopTabUI>()
                    .FirstOrDefault(x => x.ItemCategory == ItemCategory.Currency)
                    .ActivateTab();
                break;
        }
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }
}
