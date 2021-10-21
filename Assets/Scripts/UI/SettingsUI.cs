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
    [SerializeField] private Button _vibrationBtn;
    [SerializeField] private TextMeshProUGUI _vibrationState;
    [Header("Documentation")]
    [SerializeField] private Button _helpBtn;
    [SerializeField] private Button _termsOfUseBtn;
    [SerializeField] private Button _confidentialityBtn;
    [SerializeField] private CanvasGroupUI _helpWindow;
    [SerializeField] private CanvasGroupUI _termsOfUseWindow;
    [SerializeField] private CanvasGroupUI _confidentialityWindow;
    [SerializeField] private TMP_Text _confidentialityText;
    [SerializeField] private TextAsset _confidentialityTextAsset;
    [Header("Social Network")]
    [SerializeField] private Button _facebookSN;
    [SerializeField] private Button _instaSN;
    [Header("Login")]
    [SerializeField] private Button _facebookLogin;
    [SerializeField] private Button _googleLogin;
    [Header("Closures")]
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _backBtn;

    private const string FacebookLink = "https://www.facebook.com/Zombies-vs-Tower-108198291649451/";
    private const string InstagramLink = "https://www.instagram.com/zombievstower/";
    private const string EnabledState = "Enabled";
    private const string DisabledState = "Disabled";

    public new void Init()
    {
        _musicBtn.onClick.AddListener(ToggleMusic);
        _soundBtn.onClick.AddListener(ToggleSFX);
        _vibrationBtn.onClick.AddListener(ToggleVibration);

        _helpBtn.onClick.AddListener(HelpAndSupport);
        _termsOfUseBtn.onClick.AddListener(TermsOfUse);
        _confidentialityBtn.onClick.AddListener(Confidentiality);

        _facebookSN.onClick.AddListener(OpenFacebook);
        _instaSN.onClick.AddListener(OpenInstagram);

        _facebookLogin.onClick.AddListener(FacebookLogin);
        _googleLogin.onClick.AddListener(GoogleLogin);

        _exitBtn.onClick.AddListener(Hide);
        _backBtn.onClick.AddListener(Hide);

        AudioController.Instance.ToggleMusic(SLS.Data.Settings.MusicEnabled.Value);
        AudioController.Instance.ToggleSFX(SLS.Data.Settings.SoundEnabled.Value);
        _musicState.text = SLS.Data.Settings.MusicEnabled.Value == true ? EnabledState : DisabledState;
        _soundState.text = SLS.Data.Settings.SoundEnabled.Value == true ? EnabledState : DisabledState;
        _vibrationState.text = SLS.Data.Settings.VibrationEnabled.Value == true ? EnabledState : DisabledState;

        _confidentialityText.text = _confidentialityTextAsset.text;

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
        SLS.Data.Settings.MusicEnabled.Value = enabled;
    }

    private void ToggleSFX()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        bool enabled = AudioController.Instance.ToggleSFX();
        _soundState.text = enabled == true ? EnabledState : DisabledState;
        SLS.Data.Settings.SoundEnabled.Value = enabled;
    }

    private void ToggleVibration()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        SLS.Data.Settings.VibrationEnabled.Value = !SLS.Data.Settings.VibrationEnabled.Value;
        bool enabled = SLS.Data.Settings.VibrationEnabled.Value;
        _vibrationState.text = enabled == true ? EnabledState : DisabledState;

        if (enabled == true)
            Vibration.Vibrate();
    }

    private void OpenFacebook()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        Application.OpenURL(FacebookLink);
    }

    private void OpenInstagram()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        Application.OpenURL(InstagramLink);
    }

    private void FacebookLogin()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        FacebookServices.Instance.LogIn();
    }

    private void GoogleLogin()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        GPServices.Instance.LogIn();
    }

    private void HelpAndSupport()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _helpWindow.Show();
    }

    private void TermsOfUse()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _termsOfUseWindow.Show();
    }

    private void Confidentiality()
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _confidentialityWindow.Show();
    }
}
