using UnityEngine;

[System.Serializable]
public struct TowerDefenderStats
{
    public int health;
    public int damage;
    public int shootDistance;
    public int Health => health + _extraHealth;
    public int Damage => damage + _extraDamage;
    public int ShootDistance => shootDistance + _extraDistance;
    public int BasicHealth => health;
    public int BasicDamage => damage;
    public int BasicShootDistance => shootDistance;

    private int _extraHealth;
    private int _extraDamage;
    private int _extraDistance;
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
    public int ExtraDistance
    {
        get => _extraDistance;
        set
        {
            if (value < _extraDistance) return;

            _extraDistance = value;
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
                value = value % 3;
            }

            _levelProgress = value;
        }
    }
}
