public class PassiveDefensiveWeapon : DefensiveWeapon
{
    protected override void Attack(Enemy enemy)
    {
        enemy.TakeDamage(_stats.Damage);
        TakeDamage(int.MaxValue);
    }

    public override void TakeDamage(int damage)
    {
        Dead();
    }
}