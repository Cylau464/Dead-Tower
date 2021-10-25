using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkipLevelUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private int _diamondsPayCost = 10;
    [Space]
    [SerializeField] private Button[] _closeBtns;
    [SerializeField] private Button _viewAdBtn;
    [SerializeField] private Button _diamondPayBtn;
    [SerializeField] private TextMeshProUGUI _diamondCostText;

    public Action OnSkip;

    protected override void Init()
    {
        foreach (Button btn in _closeBtns)
            btn.onClick.AddListener(Hide);

        _viewAdBtn.onClick.AddListener(ViewAd);
        _viewAdBtn.interactable = AdsInitializer.Instance.RewardedAd.IsLoaded;
        _diamondPayBtn.onClick.AddListener(DiamondPay);
        _diamondPayBtn.interactable = SLS.Data.Game.Diamonds.Value >= _diamondsPayCost;
        _diamondCostText.text = _diamondsPayCost.ToString();
        AdsInitializer.Instance.RewardedAd.OnAdLoaded += OnAdLoaded;

        base.Init();
    }

    public override void Show()
    {
        base.Show();
        AdsInitializer.Instance.RewardedAd.OnAdComplete += OnAdComplete;
    }

    public override void Hide()
    {
        base.Hide();
        AdsInitializer.Instance.RewardedAd.OnAdComplete -= OnAdComplete;
    }

    private void ViewAd()
    {
        _viewAdBtn.interactable = false;
        AdsInitializer.Instance.ShowRewarded();
    }

    private void DiamondPay()
    {
        SLS.Data.Game.Diamonds.Value -= _diamondsPayCost;
        Game.Instance.ContinueLevel();
        Hide();
        OnSkip?.Invoke();
    }

    private void OnAdLoaded()
    {
        _viewAdBtn.interactable = true;
    }

    public void OnAdComplete()
    {
        Game.Instance.ContinueLevel();
        Hide();
        OnSkip?.Invoke();
    }

    private void OnDestroy()
    {
        AdsInitializer.Instance.RewardedAd.OnAdComplete -= OnAdComplete;
        AdsInitializer.Instance.RewardedAd.OnAdLoaded -= OnAdLoaded;
    }
}