using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class StatsWindowUI : MonoBehaviour
{
    [SerializeField] private Sprite _damagStatSprite;
    [SerializeField] private Sprite _healthStatSprite;
    [SerializeField] private Sprite _distanceStatSprite;
    [SerializeField] private Sprite _towerAbilityStatSprite;
    [Space]
    [SerializeField] private PercUI _percPrefab;
    [SerializeField] private StatUI _statPrefab;
    [Space]
    [SerializeField] private RectTransform _percsGroup;
    [SerializeField] private RectTransform _statsGroup;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TMP_Text _progressLevelText;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private TMP_Text _upgradeCostText;

    private List<PercUI> _percs;
    private List<StatUI> _stats;
    private UnitData _unitData;
    private bool _isInitialized;

    public void Init(UnitData data)
    {
        if (_isInitialized == true) return;

        _unitData = data;
        Dictionary<Stats, Sprite> icons = new Dictionary<Stats, Sprite>()
        {
            { Stats.Damage, _damagStatSprite },
            { Stats.Health, _healthStatSprite },
            { Stats.AttackDistance, _distanceStatSprite },
            { Stats.TowerAbility, _towerAbilityStatSprite }
        };
        _percs = new List<PercUI>(2);
        _stats = new List<StatUI>(Enum.GetValues(typeof(Stats)).Length);

        foreach (Stats stat in Enum.GetValues(typeof(Stats)))
        {
            StatUI statUI = Instantiate(_statPrefab);
            statUI.transform.SetParent(_statsGroup, false);
            statUI.Init(icons[stat], stat, data);

            if (data is TowerData)
            {
                if (stat == Stats.AttackDistance)
                    statUI.gameObject.SetActive(false);
            }
            else
            {
                if (stat == Stats.TowerAbility)
                    statUI.gameObject.SetActive(false);
            }

            _stats.Add(statUI);
        }

        Sprite[] percIcons;

        if (data is TowerData)
            percIcons = AssetsHolder.Instance.TowerConfigs[data.Index].PercIcons;
        else
            percIcons = AssetsHolder.Instance.DefenderConfigs[data.Index].PercIcons;

        for (int i = 0; i < _percs.Capacity; i++)
        {
            PercUI perc = Instantiate(_percPrefab);
            perc.transform.SetParent(_percsGroup, false);

            if (percIcons.Length > i)
                perc.SetIcon(percIcons[i]);
            else
                perc.Deactivate();

            _percs.Add(perc);
        }

        _upgradeBtn.onClick.AddListener(Upgrade);
        UpdateFields();
        SLS.Data.Game.Coins.OnValueChanged += OnCoinsChaged;

        _isInitialized = true;
    }

    private void UpdateFields()
    {
        int level = 0;
        float progress = 0f;

        if(_unitData is TowerData)
        {
            TowerData data = SLS.Data.Game.Towers.Value[_unitData.Index];
            level = data.Stats.Level;
            progress = data.Stats.LevelProgress / 3f;
        }
        else
        {
            DefenderData data = SLS.Data.Game.Defenders.Value[_unitData.Index];
            level = data.Stats.Level;
            progress = data.Stats.LevelProgress / 3f;
        }

        int upgradeCost = GetUpgradeCost();
        _upgradeBtn.interactable = SLS.Data.Game.Coins.Value >= upgradeCost;
        StopAllCoroutines();
        this.LerpCoroutine(
            time: .25f,
            from: int.Parse(_upgradeCostText.text),
            to: upgradeCost,
            action: a => _upgradeCostText.text = Mathf.Round(a).ToString()
        );
        this.LerpCoroutine(
            time: .25f,
            from: _progressSlider.value,
            to: progress,
            action: a => _progressSlider.value = a
        );
        this.LerpCoroutine(
            time: .25f,
            from: int.Parse(_progressLevelText.text),
            to: level,
            action: a => _progressLevelText.text = Mathf.Round(a).ToString()
        );
    }

    private void Upgrade()
    {
        foreach (StatUI stat in _stats)
            stat.ShowButton(GetUpgradeCost());
    }

    public void SwitchItem(UnitData data)
    {
        if (_isInitialized == false)
        {
            Init(data);
            return;
        }

        _unitData = data;
        UpdateFields();

        foreach (StatUI statUI in _stats)
        {
            statUI.HideButton();
            statUI.SwitchItem(data);
        }

        Sprite[] percIcons;

        if (data is TowerData)
            percIcons = AssetsHolder.Instance.TowerConfigs[data.Index].PercIcons;
        else
            percIcons = AssetsHolder.Instance.DefenderConfigs[data.Index].PercIcons;

        for(int i = 0; i < _percs.Count; i++)
        {
            if (percIcons.Length > i)
                _percs[i].SetIcon(percIcons[i]);
            else
                _percs[i].Deactivate();
        }
    }

    private void OnCoinsChaged(int coins)
    {
        UpdateFields();

        if(coins < GetUpgradeCost())
        {
            foreach (StatUI stat in _stats)
                stat.HideButton();
        }
    }

    private int GetUpgradeCost()
    {
        if (_unitData is TowerData)
        {
            int basicCost = AssetsHolder.Instance.TowerConfigs[_unitData.Index].PurchaseStats.StatUpgradeCost;
            return basicCost * SLS.Data.Game.Towers.Value[_unitData.Index].Stats.Level;
        }
        else
        {
            int basicCost = AssetsHolder.Instance.DefenderConfigs[_unitData.Index].PurchaseStats.StatUpgradeCost;
            return basicCost * SLS.Data.Game.Defenders.Value[_unitData.Index].Stats.Level;
        }
    }

    private void OnDestroy()
    {
        SLS.Data.Game.Coins.OnValueChanged -= OnCoinsChaged;
    }
}
