using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Level : MonoBehaviour
{
    [SerializeField] private Button _button = null;
    [SerializeField] private Sprite _closedIcon = null;
    [SerializeField] private Sprite _openedIcon = null;
    [SerializeField] private Sprite _completedIcon = null;
    private LevelConfig _config;

    public void Init(LevelConfig config)
    {
        _config = config;

        switch(config.Status)
        {
            case LevelStatus.Opened:
                _button.interactable = true;
                _button.image.sprite = _openedIcon;
                break;
            case LevelStatus.Completed:
                _button.interactable = true;
                _button.image.sprite = _completedIcon;
                break;
            case LevelStatus.Closed:
                _button.interactable = false;
                _button.image.sprite = _closedIcon;
                break;

        }

        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        if(SLS.Data.Game.ProjectilesCount.Value[SLS.Data.Game.SelectedTower.Value.Index] <= 0)
        {
            MenuSwitcher.Instance.OpenProjectileShortage();
            return;
        }

        EnemySpawner.LevelConfig = _config;
        SLS.Data.Game.LastLevel.Value = _config;
        SceneLoader.LoadLevel();
    }
}
