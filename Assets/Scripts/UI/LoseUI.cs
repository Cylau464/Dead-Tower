using UnityEngine;
using UnityEngine.UI;

public class LoseUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private SkipLevelUI _skipLevelUI;
    [SerializeField] private Button _skipBtn;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _exitBtn;

    protected override void Init()
    {
        _skipBtn.onClick.AddListener(Skip);
        _restartBtn.onClick.AddListener(SceneLoader.LoadLevel);
        _exitBtn.onClick.AddListener(SceneLoader.LoadMenu);
        _skipLevelUI.OnSkip += Hide;

        base.Init();
    }

    public override void Hide()
    {
        Time.timeScale = 1f;

        base.Hide();
    }

    private void Skip()
    {
        _skipLevelUI.Show();
    }

    private void OnDestroy()
    {
        _skipLevelUI.OnSkip -= Hide;
    }
}