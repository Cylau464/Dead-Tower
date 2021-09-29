using System;

public interface IDamageTaker
{
    public bool TakeDamage(int damage);
    public bool TakeDamage(int damage, Projectile projectile);
    public event EventHandler<float> HealthChanged;
}