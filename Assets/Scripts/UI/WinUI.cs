using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WinUI : CanvasGroupUI
{
    [Space]
    [SerializeField] private RectTransform _rewardsHolder;
    [SerializeField] private RewardUI _rewardPrefab;
    [SerializeField] private Sprite _coinSprite;

    [SerializeField] private Button _nextLevelBtn;
    [SerializeField] private Button _exitBtn;

    private List<Enemy> _enemies = new List<Enemy>();

    protected override void Init()
    {
        //_nextLevelBtn.onClick.AddListener();
        _exitBtn.onClick.AddListener(SceneLoader.LoadMenu);
        _nextLevelBtn.onClick.AddListener(NextLevel);

        base.Init();
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
        LevelConfig nextLevel = SLS.Data.Game.GetNextLevel();
        EnemySpawner.LevelConfig = nextLevel;
        SLS.Data.Game.LastLevel.Value = nextLevel;
        SceneLoader.LoadLevel();
    }
}