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
        set => _health = value;
    }
    public int Damage
    {
        get => _damage + _extraDamage;
        set => _damage = value;
    }
    public int AbilityLevel
    {
        get => _abilityLevel + _extraAL;
        set => _abilityLevel = value;
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
            if (value < 0) return;
            _extraHealth += value;
        } 
    }
    public int ExtraDamage
    {
        get => _extraDamage;
        set
        {
            if (value < 0) return;
            _extraDamage += value;
        }
    }
    public int ExtraAL
    {
        get => _extraAL;
        set
        {
            if (value < 0) return;
            _extraAL += value;
        }
    }
}