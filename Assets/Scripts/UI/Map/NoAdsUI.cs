using UnityEngine;
using UnityEngine.UI;

public class NoAdsUI : CanvasGroupUI
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _buyDiamondsBtn;

    private void Start()
    {
        _closeBtn.onClick.AddListener(Hide);
        _buyDiamondsBtn.onClick.AddListener(OpenCurrencyShop);
    }

    private void OpenCurrencyShop()
    {
        Hide();
        MenuSwitcher.Instance.HideMap();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenShop(ItemCategory.Currency);
    }
}