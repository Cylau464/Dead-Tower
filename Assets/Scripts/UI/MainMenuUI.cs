using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : CanvasGroupUI
{
    [SerializeField] private SettingsUI _settings;
    [SerializeField] private MapUI _map;
    [Space]
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Animator _animator;

    private int _showParamID;
    private int _hideParamID;

    private void Start()
    {
        _showParamID = Animator.StringToHash("show");
        _hideParamID = Animator.StringToHash("hide");

        _playBtn.onClick.AddListener(Play);
        _settingsBtn.onClick.AddListener(OpenSettings);

        _settings.OnHide += Show;
        _settings.gameObject.SetActive(false);
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
        _map.Show();
    }

    private void OpenSettings()
    {
        Hide();
        _settings.Show();
    }

    private void OnDestroy()
    {
        _settings.OnHide -= Show;
    }
}
