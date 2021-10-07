using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class TabUI : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GameObject _scrollRect;
    [SerializeField] private RectTransform _contentHolder;
    [SerializeField] private Color _activeTextColor;
    [SerializeField] private Color _inactiveTextColor;
    [Space]
    [SerializeField] private ShopItemConfig[] _itemsConfigs;
    [SerializeField] private ShopFactory _factory;

    public void Init(ToggleGroup group)
    {
        _toggle.onValueChanged.AddListener(ToggleTab);
        _toggle.group = group;
        SortConfigsAscending();

        foreach (ShopItemConfig config in _itemsConfigs)
        {
            ShopItem item = _factory.GetItem(config);
            item.transform.SetParent(_contentHolder, false);
        }
    }

    public void ToggleTab(bool active)
    {
        _scrollRect.SetActive(active);
        _title.color = active == true ? _activeTextColor : _inactiveTextColor;
    }

    private void SortConfigsAscending()
    {
        switch(_itemsConfigs[0].Category)
        {
            case ItemCategory.Tower:
            case ItemCategory.Defender:
                _itemsConfigs = _itemsConfigs.OrderBy(x => x.Stats.CurrencyType).ThenBy(x => x.Stats.Cost).ToArray();
                break;
            case ItemCategory.Currency:
                CurrencyItemConfig[] currencyConfigs = Array.ConvertAll(_itemsConfigs, item => (CurrencyItemConfig)item);
                _itemsConfigs = currencyConfigs.OrderBy(x => x.Type).ThenBy(x => x.Count).ToArray();
                break;
        }
    }
}
