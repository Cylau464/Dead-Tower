using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsUI : CanvasGroupUI
{
    [Header("Settings")]
    [SerializeField] private Button _musicBtn;
    [SerializeField] private TextMeshProUGUI _musicState;
    [SerializeField] private Button _soundBtn;
    [SerializeField] private TextMeshProUGUI _soundState;
    [Header("Social Network")]
    [SerializeField] private Button _facebookSN;
    [SerializeField] private Button _instaSN;
    [Header("Login")]
    [SerializeField] private Button _facebookLogin;
    [SerializeField] private Button _googleLogin;
    [Header("Closures")]
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _backBtn;

    private const string EnabledState = "Enabled";
    private const string DisabledState = "Disabled";

    public Action OnHide;

    private void Start()
    {
        _musicBtn.onClick.AddListener(ToggleMusic);
        _soundBtn.onClick.AddListener(ToggleSFX);

        _facebookSN.onClick.AddListener(OpenFacebook);
        _instaSN.onClick.AddListener(OpenInstagram);

        _facebookLogin.onClick.AddListener(FacebookLogin);
        _googleLogin.onClick.AddListener(GoogleLogin);

        _exitBtn.onClick.AddListener(Hide);
        _backBtn.onClick.AddListener(Hide);
    }

    public override void Hide()
    {
        base.Hide();
        OnHide?.Invoke();
    }

    private void ToggleMusic()
    {
        bool enabled = AudioController.Instance.ToggleMusic();
        _musicState.text = enabled == true ? EnabledState : DisabledState;
    }

    private void ToggleSFX()
    {
        bool enabled = AudioController.Instance.ToggleSFX();
        _soundState.text = enabled == true ? EnabledState : DisabledState;
    }

    private void OpenFacebook()
    {

    }

    private void OpenInstagram()
    {

    }

    private void FacebookLogin()
    {

    }

    private void GoogleLogin()
    {

    }
}