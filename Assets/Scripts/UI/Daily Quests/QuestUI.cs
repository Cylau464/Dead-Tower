using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Button _getBtn;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private TMP_Text _timeLeftText;
    [Space]
    [SerializeField] private AudioClip _getClip;

    private DailyQuest _quest;

    public Action<DailyQuest> OnGetReward;

    private void Update()
    {
        if (_quest != null)
        {
            _timeLeftText.text = DateTimeOffset.FromUnixTimeSeconds(_quest.TimeLeft - DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString(@"HH\:mm\:ss");
        }
    }

    public void Init(DailyQuest quest)
    {
        _quest = quest;
        _titleText.text = quest.Title;
        _toggle.isOn = quest.State == QuestStates.Completed;
        int currencyIndex = quest.RewardType == CurrencyTypes.Coins ? 1 : 0;
        _rewardText.text = $"{quest.Reward}<sprite={currencyIndex}>";
        _getBtn.interactable = quest.State == QuestStates.Completed;
        _timeLeftText.text = DateTimeOffset.FromUnixTimeSeconds(_quest.TimeLeft - DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString(@"HH\:mm\:ss");

        _getBtn.onClick.AddListener(GetReward);

        _quest.OnProgressUpdate += OnProgressUpdate;
    }

    private void GetReward()
    {
        AudioController.PlayClipAtPosition(_getClip, transform.position);

        if (_quest.RewardType == CurrencyTypes.Coins)
            SLS.Data.Game.Coins.Value += _quest.Reward;
        else
            SLS.Data.Game.Diamonds.Value += _quest.Reward;

        _getBtn.interactable = false;
        OnGetReward?.Invoke(_quest);
        Destroy(gameObject);
    }

    private void OnProgressUpdate()
    {

    }

    private void OnDestroy()
    {
        _quest.OnProgressUpdate -= OnProgressUpdate;
    }
}