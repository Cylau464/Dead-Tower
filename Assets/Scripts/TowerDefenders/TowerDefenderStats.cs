using UnityEngine;

[System.Serializable]
public struct TowerDefenderStats
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private int _shootDistance;
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
    public int ShootDistance
    {
        get => _shootDistance + _extraDistance;
        set => _shootDistance = value;
    }
    public int BasicHealth => _health;
    public int BasicDamage => _damage;
    public int BasicShootDistance => _shootDistance;

    private int _extraHealth;
    private int _extraDamage;
    private int _extraDistance;
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
    public int ExtraDistance
    {
        get => _extraDistance;
        set
        {
            if (value < 0) return;
            _extraDistance += value;
        }
    }
}
