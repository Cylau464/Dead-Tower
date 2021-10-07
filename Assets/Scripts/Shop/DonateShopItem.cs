using UnityEngine;
using UnityEngine.Purchasing;

public class DonateShopItem : ShopItem
{
    [SerializeField] private IAPButton _purchaseBtn;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        
        base.Init(config, isPurchased);
    }

    public override void Purchase()
    {

    }
}