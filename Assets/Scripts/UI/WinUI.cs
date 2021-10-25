using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class WinUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private RectTransform _rewardsHolder;
    [SerializeField] private RewardUI _rewardPrefab;
    [SerializeField] private Sprite _coinSprite;
    [Space]
    [SerializeField] private Button _nextLevelBtn;
    [SerializeField] private Button _exitBtn;

    public Action OnProjectilesEnd;

    protected override void Init()
    {
        _exitBtn.onClick.AddListener(SceneLoader.LoadMenu);
        _nextLevelBtn.onClick.AddListener(NextLevel);

        base.Init();
    }

    public override void Hide()
    {
        Time.timeScale = 1f;

        base.Hide();
    }

    public void Show(Rewards rewards)
    {
        base.Show();

        RewardUI reward = Instantiate(_rewardPrefab);
        reward.transform.SetParent(_rewardsHolder, false);
        reward.Init(_coinSprite, rewards.Gold);

        SLS.Data.Game.Coins.Value += rewards.Gold;
        Resource[] resources = SLS.Data.Game.Resources.Value;

        foreach (Resource res in rewards.Resources)
        {
            reward = Instantiate(_rewardPrefab);
            reward.transform.SetParent(_rewardsHolder, false);
            Sprite icon = AssetsHolder.Instance.ResourceConfigs.FirstOrDefault(x => x.Type == res.Type).RewardIcon;
            reward.Init(icon, res.Count);

            int index = resources.ToList().IndexOf(resources.FirstOrDefault(x => x.Type == res.Type));
            resources[index].Count += res.Count;
        }

        SLS.Data.Game.Resources.Value = resources;
    }

    private void NextLevel()
    {
        if (SLS.Data.Game.ProjectilesCount.Value[SLS.Data.Game.SelectedTower.Value.Index] <= 0)
        {
            OnProjectilesEnd?.Invoke();
            return;
        }

        Time.timeScale = 1f;
        LevelConfig nextLevel = SLS.Data.Game.GetNextLevel();
        EnemySpawner.LevelConfig = nextLevel;
        SLS.Data.Game.LastLevel.Value = nextLevel;
        SceneLoader.LoadLevel();
    }
}