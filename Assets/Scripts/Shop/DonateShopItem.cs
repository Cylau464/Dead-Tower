using UnityEngine;
using UnityEngine.Purchasing;

public class DonateShopItem : ShopItem
{
    [SerializeField] private IAPButton _purchaseBtn;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        _config = config;

    }

    public override void Purchase()
    {

    }
}