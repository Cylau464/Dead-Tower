using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ShopTabUI : TabUI
{
    [Space]
    [SerializeField] private ShopItemConfig[] _itemsConfigs;
    [SerializeField] private ShopFactory _factory;
    [SerializeField] private ItemCategory _itemCategory;
    public ItemCategory ItemCategory => _itemCategory;

    public override void Init(ToggleGroup group)
    {
        base.Init(group);

        foreach (ShopItemConfig config in _itemsConfigs)
        {
            ShopItem item = _factory.GetItem(config);
            item.transform.SetParent(_contentHolder, false);
        }
    }

    protected override void SortAscendingConfigs()
    {
        switch (_itemsConfigs[0].Category)
        {
            case ItemCategory.Tower:
            case ItemCategory.Defender:
            case ItemCategory.Resources:
                _itemsConfigs = _itemsConfigs.OrderBy(x => x.Stats.CurrencyType).ThenBy(x => x.Stats.Cost).ToArray();
                break;
            case ItemCategory.Currency:
                CurrencyItemConfig[] currencyConfigs = Array.ConvertAll(_itemsConfigs, item => (CurrencyItemConfig)item);
                _itemsConfigs = currencyConfigs.OrderBy(x => x.Type).ThenBy(x => x.Count).ToArray();
                break;
        }
    }
}
