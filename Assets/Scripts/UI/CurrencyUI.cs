using System.Collections;
using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _diamondsText;
    [SerializeField] private float _accuralTime = .5f;
    [SerializeField] private float _scaleIncrease = .2f;

    private void Start()
    {
        _coinsText.text = SLS.Data.Game.Coins.Value.ToString();
        _diamondsText.text = SLS.Data.Game.Diamonds.Value.ToString();
        SLS.Data.Game.Coins.OnValueChanged += UpdateCoins;
        SLS.Data.Game.Diamonds.OnValueChanged += UpdateDiamonds;
    }

    private void UpdateCoins(int newValue)
    {
        if(gameObject.activeInHierarchy == false)
        {
            _coinsText.text = newValue.ToString();
            return;
        }

        StopCoroutine(AccuralCor(_coinsText, newValue));
        StartCoroutine(AccuralCor(_coinsText, newValue));
    }
    
    private void UpdateDiamonds(int newValue)
    {
        if (gameObject.activeInHierarchy == false)
        {
            _diamondsText.text = newValue.ToString();
            return;
        }

        StopCoroutine(AccuralCor(_diamondsText, newValue));
        StartCoroutine(AccuralCor(_diamondsText, newValue));
    }

    private IEnumerator AccuralCor(TextMeshProUGUI text, int targetValue)
    {
        int startValue = int.Parse(text.text);
        Vector3 scaleTarget = Vector3.one + Vector3.one * _scaleIncrease;
        float t = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime / _accuralTime;
            text.text = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t)).ToString();
            text.transform.localScale = Vector3.one + Vector3.one * Mathf.PingPong(t * _scaleIncrease * 2f, _scaleIncrease);

            yield return null;
        }

        text.text = targetValue.ToString();
        text.transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        SLS.Data.Game.Coins.OnValueChanged -= UpdateCoins;
        SLS.Data.Game.Diamonds.OnValueChanged -= UpdateDiamonds;
    }
}
