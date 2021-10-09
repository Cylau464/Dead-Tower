using UnityEngine;

[CreateAssetMenu(fileName = "Shop Factory", menuName = "Factories/UI/Shop Factory")]
public class ShopFactory : GameObjectFactory
{
    public ShopItem GetItem(ShopItemConfig config)
    {
        switch (config.Category)
        {
            case ItemCategory.Tower:
                return GetTower(config as TowerItemConfig);
            case ItemCategory.Defender:
                return GetDefender(config as DefenderItemConfig);
            case ItemCategory.Resources:
                return GetResources(config as ResourcesItemConfig);
            case ItemCategory.Currency:
                return GetCurrency(config as CurrencyItemConfig);
            default:
                return null;
        }
    }

    private ShopItem GetTower(TowerItemConfig config)
    {
        ShopItem item = CreateGameObjectInstance(config.Prefab);
        item.Init(config, SLS.Data.Game.Towers.Value[config.Config.Index].IsPurchased);
        return item;
    }

    private ShopItem GetDefender(DefenderItemConfig config)
    {
        ShopItem item = CreateGameObjectInstance(config.Prefab);
        item.Init(config, SLS.Data.Game.Defenders.Value[config.Config.Index].IsPurchased);
        return item;
    }

    private ShopItem GetResources(ResourcesItemConfig config)
    {
        ShopItem item = CreateGameObjectInstance(config.Prefab);
        item.Init(config);
        return item;
    }

    private ShopItem GetCurrency(CurrencyItemConfig config)
    {
        ShopItem item = CreateGameObjectInstance(config.Prefab);
        item.Init(config);
        return item;
    }
}