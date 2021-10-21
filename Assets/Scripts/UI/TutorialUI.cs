using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : CanvasGroupUI
{
    [SerializeField] private CutoutMaskUI _mask;

    private Weapon _weapon;

    public void Init(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.OnAimStart += Hide;
        _mask.enabled = false;

        base.Init();
    }

    public override void Show()
    {
        Time.timeScale = 0f;
        base.Show();
        this.DoAfterNextFrameCoroutine(() => _mask.enabled = true);
    }

    public override void Hide()
    {
        Time.timeScale = 1f;
        SLS.Data.Settings.TutorialPassed.Value = true;
        _weapon.OnAimStart -= Hide;

        base.Hide();
    }

    private void OnDestroy()
    {
        _weapon.OnAimStart -= Hide;
    }
}