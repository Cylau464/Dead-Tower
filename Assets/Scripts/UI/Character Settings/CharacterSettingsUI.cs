using UnityEngine;
using UnityEngine.UI;

public class CharacterSettingsUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private Button _closeBtn;

    private void Start()
    {
        _closeBtn.onClick.AddListener(Close);
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }
}
