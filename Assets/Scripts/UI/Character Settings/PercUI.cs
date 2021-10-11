using UnityEngine;
using UnityEngine.UI;

public class PercUI : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
        gameObject.SetActive(true);

        StopAllCoroutines();
        this.LerpCoroutine(
            time: .125f,
            from: Vector3.one,
            to: Vector3.zero,
            action: a => transform.localScale = a,
            settings: new CoroutineTemplate.Settings(false, 0, true)
        );
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}