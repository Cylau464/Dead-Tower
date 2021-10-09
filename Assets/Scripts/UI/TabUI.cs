using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TabUI : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] protected ScrollRect _scrollRect;
    [SerializeField] protected RectTransform _contentHolder;
    [SerializeField] private Color _activeTextColor;
    [SerializeField] private Color _inactiveTextColor;
    [SerializeField] protected ItemCategory _itemCategory;
    public ItemCategory ItemCategory => _itemCategory;

    protected bool _isInitialized;

    public virtual void Init(ToggleGroup group)
    {
        if (_isInitialized == true) return;
        _toggle.onValueChanged.AddListener(ToggleTab);
        _toggle.group = group;
        SortAscendingConfigs();
        _isInitialized = true;
    }

    protected virtual void ToggleTab(bool active)
    {
        _scrollRect.gameObject.SetActive(active);
        _title.color = active == true ? _activeTextColor : _inactiveTextColor;
    }

    public void ActivateTab()
    {
        _toggle.isOn = true;
    }

    protected void ScrollTo(ScrollRect scroll, float offset)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            this.LerpCoroutine(
                time: .2f,
                from: scroll.normalizedPosition.x,
                to: offset,
                action: a => scroll.normalizedPosition = new Vector2(a, scroll.normalizedPosition.y)
            );
        }
        else
        {
            scroll.normalizedPosition = new Vector2(offset, scroll.normalizedPosition.y);
        }
    }

    public abstract void ScrollTo(int itemIndex);
    protected abstract void SortAscendingConfigs();
}
