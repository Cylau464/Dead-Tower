using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Animator _animator;

    private int _showParamID;
    private int _hideParamID;

    protected override void Init()
    {
        _showParamID = Animator.StringToHash("show");
        _hideParamID = Animator.StringToHash("hide");

        _playBtn.onClick.AddListener(Play);
        _settingsBtn.onClick.AddListener(OpenSettings);

        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
        _animator.SetTrigger(_hideParamID);
    }

    public override void Show()
    {
        base.Show();
        _animator.SetTrigger(_showParamID);
    }

    private void Play()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }

    private void OpenSettings()
    {
        Hide();
        MenuSwitcher.Instance.OpenSettings();
    }
}
