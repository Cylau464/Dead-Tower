using System;
using System.Collections.Generic;

[Serializable]
public class QuestsData
{
    public StoredValue<long> CooldownTime;
    public StoredValue<List<DailyQuest>> Quests;

    public QuestsData()
    {
        CooldownTime = new StoredValue<long>(0);
        Quests = new StoredValue<List<DailyQuest>>(new List<DailyQuest>());
    }
}