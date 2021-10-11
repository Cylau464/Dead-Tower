using UnityEngine;

[System.Serializable]
public struct TowerStats
{
    public int health;
    public int damage;
    public int abilityLevel;
    public int Health => health + _extraHealth;
    public int Damage => damage + _extraDamage;
    public int AbilityLevel => abilityLevel + _extraAL;
    public int BasicHealth => health;
    public int BasicDamage => damage;
    public int BasicAbilityLevel => abilityLevel;

    private int _extraHealth;
    private int _extraDamage;
    private int _extraAL;
    public int ExtraHealth
    {
        get => _extraHealth;
        set 
        {
            if (value < _extraHealth) return;

            _extraHealth = value;
        } 
    }
    public int ExtraDamage
    {
        get => _extraDamage;
        set
        {
            if (value < _extraDamage) return;

            _extraDamage = value;
        }
    }
    public int ExtraAL
    {
        get => _extraAL;
        set
        {
            if (value < _extraAL) return;

            _extraAL = value;
        }
    }

    private int _level;
    public int Level
    {
        get => Mathf.Max(_level, 1);
        set
        {
            if (value < _level) return;

            _level = value;
        }
    }
    private int _levelProgress;
    public int LevelProgress
    {
        get => _levelProgress;
        set
        {
            if (value < _levelProgress) return;

            if (value >= 3)
            {
                Level += value / 3;
                value %= 3;
            }

            _levelProgress = value;
        }
    }
}