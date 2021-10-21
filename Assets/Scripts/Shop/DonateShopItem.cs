using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using System.Linq;

public class DonateShopItem : ShopItem
{
    [SerializeField] private IAPButton _purchaseBtn;
    [SerializeField] private TMP_Text _countText;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        base.Init(config, isPurchased);

        _purchaseBtn.productId = (config as DiamondItemConfig).StoreItemID;
        Product product = StoreListener.Instance.StoreController.products.WithID(_purchaseBtn.productId);
        _costText.text = product.metadata.localizedPrice.ToString() + " " + product.metadata.isoCurrencyCode;
        _countText.text = product.definition.payout.quantity.ToString();
        _purchaseBtn.onPurchaseComplete.AddListener(Purchase);
    }

    public void Purchase(Product product)
    {
        SLS.Data.Game.Diamonds.Value += (int) product.definition.payout.quantity;
        SLS.Data.Settings.AdsEnabled.Value = false;
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    public override void Purchase() { }
}