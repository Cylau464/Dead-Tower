using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipLevelUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private int _diamondsPayCost = 10;
    [Space]
    [SerializeField] private Button[] _closeBtns;
    [SerializeField] private Button _viewAdBtn;
    [SerializeField] private Button _diamondPayBtn;
    [SerializeField] private TextMeshProUGUI _diamondCostText;

    protected override void Init()
    {
        foreach (Button btn in _closeBtns)
            btn.onClick.AddListener(Hide);

        _viewAdBtn.onClick.AddListener(ViewAd);
        _diamondPayBtn.onClick.AddListener(DiamondPay);
        _diamondCostText.text = _diamondsPayCost.ToString();

        base.Init();
    }

    private void ViewAd()
    {

    }

    private void DiamondPay()
    {

    }
}