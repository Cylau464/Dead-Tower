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

        base.Init();
    }

    private void Skip()
    {
        _skipLevelUI.Show();
    }
}