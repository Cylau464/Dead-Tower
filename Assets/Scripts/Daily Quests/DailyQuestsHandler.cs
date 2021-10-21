using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class DailyQuestsHandler : MonoBehaviour
{
    [SerializeField] private DailyQuestConfig[] _configs;
    [SerializeField] private int _cooldown = 86400;
    [SerializeField] private int _questsCount = 4;
    private DateTime _nextQuestTime;
    private List<DailyQuest> _quests;
    private int _prevGold;

    public static DailyQuestsHandler Instance;

    public Action OnUpdateQuests;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _prevGold = SLS.Data.Game.Coins.Value;

        if (SLS.Data.Quests.CooldownTime.Value <= 0)
        {
            _quests = new List<DailyQuest>(_questsCount);
            _nextQuestTime = DateTime.UtcNow;
        }
        else
        {
            _quests = SLS.Data.Quests.Quests.Value;
            _nextQuestTime = TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeSeconds(SLS.Data.Quests.CooldownTime.Value).DateTime, TimeZoneInfo.Local);
        }

        StartCoroutine(CheckCooldownCor());

        SLS.Data.Game.Coins.OnValueChanged += OnGoldChanged;
    }

    private void AddQuests()
    {
        _quests = new List<DailyQuest>(_questsCount);
        _nextQuestTime = DateTime.UtcNow + new TimeSpan(0, 0, _cooldown);

        for(int i = 0; i < _questsCount; i++)
        {
            _quests.Add(new DailyQuest(_configs[Random.Range(0, _configs.Length)], ((DateTimeOffset) _nextQuestTime).ToUnixTimeSeconds()));
        }
        
        SLS.Data.Quests.Quests.Value = _quests;
        SLS.Data.Quests.CooldownTime.Value = ((DateTimeOffset) _nextQuestTime).ToUnixTimeSeconds();
        OnUpdateQuests?.Invoke();
    }

    private IEnumerator CheckCooldownCor()
    {
        while(true)
        {
            TimeSpan timeDifference = _nextQuestTime - DateTime.UtcNow;

            if (timeDifference.TotalSeconds <= 0)
                AddQuests();

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private void OnGoldChanged(int value)
    {
        if (_prevGold < value)
        {
            List<DailyQuest> quests = SLS.Data.Quests.Quests.Value;
            quests = quests.Select(x =>
            {
                if (x.Type == QuestTypes.Income)
                    x.AddProgress(value - _prevGold);

                return x;
            }).ToList();

            SLS.Data.Quests.Quests.Value = quests;
            SLS.Data.Quests.Quests.SaveValue();
        }

        _prevGold = value;
    }

    public void EnemyDead(KillTargets type)
    {
        List<DailyQuest> quests = SLS.Data.Quests.Quests.Value;
        quests = quests.Select(x =>
        {
            if (x.Type == QuestTypes.Kill && x.TargetType == type)
                x.AddProgress(1);

            return x;
        }).ToList();

        SLS.Data.Quests.Quests.Value = quests;
        SLS.Data.Quests.Quests.SaveValue();
    }

    public void LevelComplete()
    {
        List<DailyQuest> quests = SLS.Data.Quests.Quests.Value;

        quests = quests.Select(x =>
        {
            if (x.Type == QuestTypes.Passing)
                x.AddProgress(1);
            
            return x;
        }).ToList();

        SLS.Data.Quests.Quests.Value = quests;
        SLS.Data.Quests.Quests.SaveValue();
    }

    private void OnDestroy()
    {
        SLS.Data.Game.Coins.OnValueChanged -= OnGoldChanged;
    }
}