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

    protected abstract void SortAscendingConfigs();
}
