using UnityEngine;
using UnityEngine.UI;

public class PauseUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _exitBtn;
    [Space]
    [SerializeField] private Animator _animator;

    private int _showParamID;
    private int _hideParamID;

    protected override void Init()
    {
        _showParamID = Animator.StringToHash("show");
        _hideParamID = Animator.StringToHash("hide");

        _resumeBtn.onClick.AddListener(Resume);
        _exitBtn.onClick.AddListener(ExitToMap);

        base.Init();
    }

    public override void Show()
    {
        Time.timeScale = 0f;
        base.Show();
        _animator.SetTrigger(_showParamID);
    }

    public override void Hide()
    {
        base.Hide();
        _animator.SetTrigger(_hideParamID);
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        Hide();
    }

    private void ExitToMap()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadMenu();
    }
}
