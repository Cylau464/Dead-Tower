using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _diamondsText;

    private void Start()
    {
        _coinsText.text = SLS.Data.Game.Coins.Value.ToString();
        _diamondsText.text = SLS.Data.Game.Diamonds.Value.ToString();
        SLS.Data.Game.Coins.OnValueChanged += UpdateCoins;
        SLS.Data.Game.Diamonds.OnValueChanged += UpdateDiamonds;
    }

    private void UpdateCoins(int newValue)
    {
        _coinsText.text = newValue.ToString();
    }
    
    private void UpdateDiamonds(int newValue)
    {
        _diamondsText.text = newValue.ToString();
    }
}
