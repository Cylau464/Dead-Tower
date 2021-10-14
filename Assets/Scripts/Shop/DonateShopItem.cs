using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class DonateShopItem : ShopItem
{
    [SerializeField] private IAPButton _purchaseBtn;
    [SerializeField] private TMP_Text _countText;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        base.Init(config, isPurchased);
        _purchaseBtn.productId = (config as DiamondItemConfig).StoreItemID;
        //Product product = StoreListener.Instance.StoreController.products.WithID((config as DiamondItemConfig).StoreItemID);
        //_costText.text = product.metadata.localizedPrice.ToString() + product.metadata.localizedTitle;
        //_countText.text = product.definition.payout.quantity.ToString();
        _purchaseBtn.onPurchaseComplete.AddListener(Purchase);
    }

    public void Purchase(Product product)
    {
        SLS.Data.Game.Diamonds.Value += (int) product.definition.payout.quantity;
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    public override void Purchase() { }
}