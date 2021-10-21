using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DailyQuestsUI : CanvasGroupUI
{
    [SerializeField] private QuestUI _questPrefab;
    [SerializeField] private RectTransform _questHolder;
    [SerializeField] private Button _closeBtn;

    private List<QuestUI> _questsUI;

    protected override void Init()
    {
        _closeBtn.onClick.AddListener(Close);
        SpawnQuests();
        DailyQuestsHandler.Instance.OnUpdateQuests += OnUpdateQuests;

        base.Init();
    }

    private void SpawnQuests()
    {
        _questsUI = new List<QuestUI>(SLS.Data.Quests.Quests.Value.Count);

        foreach (DailyQuest quest in SLS.Data.Quests.Quests.Value)
        {
            _questsUI.Add(Instantiate(_questPrefab));
            _questsUI[_questsUI.Count - 1].transform.SetParent(_questHolder, false);
            _questsUI[_questsUI.Count - 1].Init(quest);
            _questsUI[_questsUI.Count - 1].OnGetReward += OnGetReward;
        }
    }

    private void Close()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }

    private void OnGetReward(DailyQuest quest)
    {
        SLS.Data.Quests.Quests.Value.Remove(quest);
        SLS.Data.Quests.Quests.SaveValue();
    }

    private void OnUpdateQuests()
    {
        foreach(RectTransform transform in _questHolder)
            Destroy(transform.gameObject);

        SpawnQuests();
    }

    private void OnDestroy()
    {
        DailyQuestsHandler.Instance.OnUpdateQuests -= OnUpdateQuests;

        foreach (QuestUI questUI in _questsUI)
        {
            questUI.OnGetReward -= OnGetReward;
        }
    }
}