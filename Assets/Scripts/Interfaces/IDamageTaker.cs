public interface IDamageTaker
{
    public bool TakeDamage(int damage);
    public bool TakeDamage(int damage, Projectile projectile);
}