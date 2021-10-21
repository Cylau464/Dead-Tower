using UnityEngine;

public enum KillTargets { Simple, Great, Archer, Armored, Axe, Boss }

[CreateAssetMenu(fileName = "Kill Quest", menuName = "Quests/Kill")]
public class KillQuestConfig : DailyQuestConfig
{
    public KillTargets Target;
}