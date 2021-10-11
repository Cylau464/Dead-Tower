using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Stats { Damage, Health, AttackDistance, TowerAbility }

public class StatUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private TMP_Text _valueText;

    private Stats _stat;
    public Stats Stat => _stat;
    private UnitData _unitData;
    private int _curCost;

    public void Init(Sprite icon, Stats stat, UnitData data)
    {
        _icon.sprite = icon;
        _stat = stat;
        _unitData = data;
        _upgradeBtn.onClick.AddListener(Upgrade);

        Color color = _upgradeBtn.targetGraphic.color;
        color.a = 0f;
        _upgradeBtn.targetGraphic.color = color;
        _upgradeBtn.interactable = false;
        SetCountToText(GetStatValue());
    }

    public void SwitchItem(UnitData data)
    {
        if (_stat == Stats.TowerAbility)
            gameObject.SetActive(data is TowerData);
        else if (_stat == Stats.AttackDistance)
            gameObject.SetActive(data is DefenderData);

        _unitData = data;
        SetCountToText(GetStatValue());
    }

    private void SetCountToText(int count)
    {
        if (gameObject.activeInHierarchy == false)
        {
            _valueText.text = count.ToString();
        }
        else
        {
            this.LerpCoroutine(
                time: .25f,
                from: int.Parse(_valueText.text),
                to: count,
                action: a => _valueText.text = Mathf.Round(a).ToString()
            );
        }
    }

    private int GetStatValue(bool onlyExtra = false)
    {
        if(_unitData is TowerData)
        {
            TowerData data = _unitData as TowerData;

            switch (_stat)
            {
                case Stats.Damage:
                    return onlyExtra == true ? data.Stats.ExtraDamage : data.Stats.Damage;
                case Stats.Health:
                    return onlyExtra == true ? data.Stats.ExtraHealth : data.Stats.Health;
                case Stats.TowerAbility:
                    return onlyExtra == true ? data.Stats.ExtraAL : data.Stats.AbilityLevel;
            }

            return int.Parse(_valueText.text);
        }
        else
        {
            DefenderData data = _unitData as DefenderData;

            switch(_stat)
            {
                case Stats.Damage:
                    return onlyExtra == true ? data.Stats.ExtraDamage : data.Stats.Damage;
                case Stats.Health:
                    return onlyExtra == true ? data.Stats.ExtraHealth : data.Stats.Health;
                case Stats.AttackDistance:
                    return onlyExtra == true ? data.Stats.ExtraDistance : data.Stats.ShootDistance;
            }

            return int.Parse(_valueText.text);
        }
    }

    private void Upgrade()
    {
        int newValue = IncreaseStat();

        if (GetStatValue(true) >= 5)
            HideButton();

        SetCountToText(newValue);
        SLS.Data.Game.Coins.Value -= _curCost;
    }

    private int IncreaseStat()
    {
        int value = 0;

        if (_unitData is TowerData)
        {
            TowerData data = SLS.Data.Game.Towers.Value[_unitData.Index];

            switch (_stat)
            {
                case Stats.Damage:
                    value = ++data.Stats.ExtraDamage +data.Stats.Damage;
                    break;
                case Stats.Health:
                    value = ++data.Stats.ExtraHealth + data.Stats.BasicHealth;
                    break;
                case Stats.TowerAbility:
                    value = ++data.Stats.ExtraAL + data.Stats.BasicAbilityLevel;
                    break;
            }

            data.Stats.LevelProgress++;
            SLS.Data.Game.Towers.SaveValue();

            return value;
        }
        else
        {
            DefenderData data = SLS.Data.Game.Defenders.Value[_unitData.Index];

            switch (_stat)
            {
                case Stats.Damage:
                    value = ++data.Stats.ExtraDamage + data.Stats.BasicDamage;
                    break;
                case Stats.Health:
                    value = ++data.Stats.ExtraHealth + data.Stats.BasicHealth;
                    break;
                case Stats.AttackDistance:
                    value = ++data.Stats.ExtraDistance + data.Stats.BasicShootDistance;
                    break;
            }

            data.Stats.LevelProgress++;
            SLS.Data.Game.Defenders.SaveValue();

            return value;
        }
    }

    public void ShowButton(int cost)
    {
        if(GetStatValue(true) >= 5 || gameObject.activeInHierarchy == false) return;

        _curCost = cost;
        _upgradeBtn.interactable = true;
        StopAllCoroutines();
        this.ChangeAlpha(
            time: .25f,
            graphic: _upgradeBtn.targetGraphic,
            to: 1f
        );
    }

    public void HideButton()
    {
        if (gameObject.activeInHierarchy == false) return;

        _upgradeBtn.interactable = false;
        StopAllCoroutines();
        this.ChangeAlpha(
            time: .25f,
            graphic: _upgradeBtn.targetGraphic,
            to: 0f
        );
    }
}