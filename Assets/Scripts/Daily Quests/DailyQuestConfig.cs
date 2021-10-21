using UnityEngine;

public enum QuestTypes { Kill, Income, Passing }

[System.Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Basic")]
public class DailyQuestConfig : ScriptableObject
{
    public QuestTypes Type;
    public int MinAmount;
    public int MaxAmount;
    [Space]
    public CurrencyTypes RewardType;
    public int MinReward;
    public int MaxReward;
}