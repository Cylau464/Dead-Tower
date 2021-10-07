using UnityEngine;
using TMPro;

public class ResourcesShopItem : CommonShopItem
{
    [SerializeField] private TextMeshProUGUI _countText;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        _countText.text = (config as MPIConfig).Count.ToString();
        base.Init(config, isPurchased);
    }
}