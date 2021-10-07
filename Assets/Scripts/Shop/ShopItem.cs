using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopItem : MonoBehaviour
{
    [SerializeField] protected Material _grayscaleMaterial;
    [Space]
    [SerializeField] protected Image _itemIcon;
    [SerializeField] protected Image _currencyIcon;
    [SerializeField] protected Image _gradient;
    [SerializeField] protected TextMeshProUGUI _costText;

    protected ShopItemConfig _config;

    protected virtual void Disable()
    {
        _gradient.material = _grayscaleMaterial;
        _currencyIcon.material = _grayscaleMaterial;
        _itemIcon.material = _grayscaleMaterial;
    }

    public abstract void Init(ShopItemConfig config, bool isPurchased = false);
    public abstract void Purchase();
}