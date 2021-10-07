using UnityEngine;
using UnityEngine.UI;

public class CommonShopItem : ShopItem
{
    [SerializeField] private Sprite _coinSprite;
    [SerializeField] private Sprite _diamondSprite;
    [Space]
    [SerializeField] private Button _purchaseBtn;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        _config = config;
        _itemIcon.sprite = config.Icon;
        _currencyIcon.sprite = config.Stats.CurrencyType == CurrencyTypes.Coins ? _coinSprite : _diamondSprite;
        _costText.text = config.Stats.Cost.ToString();

        _purchaseBtn.onClick.AddListener(Purchase);

        if(isPurchased == true)
            Disable();
    }

    protected override void Disable()
    {
        base.Disable();
        _purchaseBtn.interactable = false;
    }

    public override void Purchase()
    {

    }
}