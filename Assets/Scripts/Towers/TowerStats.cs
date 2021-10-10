using UnityEngine;

[System.Serializable]
public struct TowerStats
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private int _abilityLevel;
    public int Health
    {
        get => _health + _extraHealth;
        set => _health = value - _extraHealth;
    }
    public int Damage
    {
        get => _damage + _extraDamage;
        set => _damage = value - _extraDamage;
    }
    public int AbilityLevel
    {
        get => _abilityLevel + _extraAL;
        set => _abilityLevel = value - _extraAL;
    }
    public int BasicHealth => _health;
    public int BasicDamage => _damage;
    public int BasicAbilityLevel => _abilityLevel;

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