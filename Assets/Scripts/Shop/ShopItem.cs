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

    protected virtual void Purchased()
    {
        _gradient.material = _grayscaleMaterial;
        _itemIcon.material = _grayscaleMaterial;
        _currencyIcon.gameObject.SetActive(false);
        _costText.gameObject.SetActive(false);
    }

    public virtual void Init(ShopItemConfig config, bool isPurchased = false)
    {
        _config = config;
        _itemIcon.sprite = config.Icon;

        if (isPurchased == true)
            Purchased();
    }

    public abstract void Purchase();
}