public class PassiveDefensiveWeapon : DefensiveWeapon
{
    protected override void Attack(Enemy enemy)
    {
        enemy.TakeDamage(_stats.Damage);
        TakeDamage(int.MaxValue);
    }

    public override bool TakeDamage(int damage)
    {
        _stats.Health = 0;
        OnHealthChange();
        Dead();

        return true;
    }

    public override bool TakeDamage(int damage, Projectile projectile)
    {
        bool dead = TakeDamage(damage);

        if (dead == false)
            _stuckProjectiles.Add(projectile);

        return dead;
    }
}