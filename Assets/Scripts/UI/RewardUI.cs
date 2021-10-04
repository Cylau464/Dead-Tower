using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(Sprite icon, int count)
    {
        _icon.sprite = icon;
        _text.text = count.ToString();
    }
}