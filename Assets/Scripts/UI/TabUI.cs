using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabUI : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GameObject _viewport;
    [SerializeField] private RectTransform _contentHolder;
    [SerializeField] private Color _activeTextColor;
    [SerializeField] private Color _inactiveTextColor;
    [Space]
    [SerializeField] private ShopItemConfig[] _itemsConfigs;
    [SerializeField] private ShopFactory _factory;

    public void Init(ToggleGroup group)
    {
        _toggle.onValueChanged.AddListener(ToggleTab);
        _toggle.group = group;

        foreach(ShopItemConfig config in _itemsConfigs)
        {
            ShopItem item = _factory.GetItem(config);
            item.transform.SetParent(_contentHolder, false);
        }
    }

    public void ToggleTab(bool active)
    {
        _viewport.SetActive(active);
        _title.color = active == true ? _activeTextColor : _inactiveTextColor;
    }
}
