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

    protected override void Init()
    {
        _musicBtn.onClick.AddListener(ToggleMusic);
        _soundBtn.onClick.AddListener(ToggleSFX);

        _facebookSN.onClick.AddListener(OpenFacebook);
        _instaSN.onClick.AddListener(OpenInstagram);

        _facebookLogin.onClick.AddListener(FacebookLogin);
        _googleLogin.onClick.AddListener(GoogleLogin);

        _exitBtn.onClick.AddListener(Hide);
        _backBtn.onClick.AddListener(Hide);

        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenStartMenu();
    }

    private void ToggleMusic()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        bool enabled = AudioController.Instance.ToggleMusic();
        _musicState.text = enabled == true ? EnabledState : DisabledState;
    }

    private void ToggleSFX()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        bool enabled = AudioController.Instance.ToggleSFX();
        _soundState.text = enabled == true ? EnabledState : DisabledState;
    }

    private void OpenFacebook()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    private void OpenInstagram()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    private void FacebookLogin()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }

    private void GoogleLogin()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
    }
}
