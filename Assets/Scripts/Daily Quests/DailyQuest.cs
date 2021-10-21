using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum QuestStates { Acitve, Completed, Failed }

[Serializable]
public class DailyQuest
{
    public QuestStates State;
    public QuestTypes Type;
    public KillTargets TargetType;
    public string Title;
    public long TimeLeft;
    public int Reward;
    public CurrencyTypes RewardType;
    public int Progress;
    public int TargetAmount;

    [Newtonsoft.Json.JsonIgnore]
    public Action OnProgressUpdate;

    public DailyQuest()
    {

    }

    public DailyQuest(DailyQuestConfig config, long timeLeft)
    {
        Type = config.Type;
        TargetType = config is KillQuestConfig ? (config as KillQuestConfig).Target : default;
        State = QuestStates.Acitve;
        TimeLeft = timeLeft;
        Progress = 0;
        float random = Random.value;
        Reward = RoundToNearest10or100(Mathf.Lerp(config.MinReward, config.MaxReward, random));
        RewardType = config.RewardType;

        if (Type == QuestTypes.Income)
            TargetAmount = RoundToNearest10or100(Mathf.Lerp(config.MinAmount, config.MaxAmount, random));
        else
            TargetAmount = Mathf.RoundToInt(Mathf.Lerp(config.MinAmount, config.MaxAmount, random));

        switch (config.Type)
        {
            case QuestTypes.Kill:
                string name = (config as KillQuestConfig).Target.ToString();
                Title = $"Kill {TargetAmount} {name} Zombie";
                break;
            case QuestTypes.Income:
                Title = $"Earn {TargetAmount} gold";
                break;
            case QuestTypes.Passing:
                Title = $"Complete {TargetAmount} levels";
                break;
        }
    }

    public void AddProgress(int progress)
    {
        if (progress < 0)
            throw new Exception("Try to add negative progress");

        Progress = Mathf.Min(Progress + progress, TargetAmount);

        if (Progress >= TargetAmount)
            State = QuestStates.Completed;

        OnProgressUpdate?.Invoke();
    }

    private int RoundToNearest10or100(float value)
    {
        if (value > 100f)
            return Mathf.RoundToInt(value / 100) * 100;
        else
            return Mathf.RoundToInt(value / 10) * 10;
    }
}