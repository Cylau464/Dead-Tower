using UnityEngine;

public class PassiveDefensiveWeapon : DefensiveWeapon
{
    [SerializeField] private float _explosionRadius = 2f;
    [SerializeField] private float _rotationMultiplier = 10f;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.back, _rigidBody.velocity.magnitude * _rotationMultiplier * Time.deltaTime);
    }

    protected override void Attack(Enemy enemy)
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _targetLayer);

        foreach (Collider2D col in colliders)
        {
            if (col.TryGetComponent(out Enemy enemy) == true)
                enemy.TakeDamage(int.MaxValue);
        }

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