using UnityEngine;
using UnityEngine.UI;

public class PercUI : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}