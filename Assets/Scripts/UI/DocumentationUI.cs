using UnityEngine;
using UnityEngine.UI;

public class DocumentationUI : CanvasGroupUI
{
    [SerializeField] private Button[] _closeBtn;

    private void Start()
    {
        foreach(Button button in _closeBtn)
            button.onClick.AddListener(Close);
    }

    private void Close()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        base.Hide();
    }
}