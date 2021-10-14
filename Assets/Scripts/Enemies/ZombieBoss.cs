using UnityEngine;
using System.Collections;

public class ZombieBoss : Enemy
{
    [Space]
    [SerializeField] private float _attackCooldown = 3f;
    [SerializeField] private float _projectileSpeed = 5f;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPos;

    private ProjectileStats _projectileStats;

    private IEnumerator AttackCor()
    {
        while(true)
        {
            yield return new WaitForSeconds(_attackCooldown);

            Attack();
        }
    }

    private void Shoot()
    {
        Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPos.position, Quaternion.identity);
        projectile.transform.right = Vector3.left;
        projectile.Init(_projectileStats);
        projectile.Launch(Vector2.left * _projectileSpeed);
    }

    protected override void Attack()
    {
        _animator.SetTrigger(_attackParamID);
    }


    protected override void StopMove()
    {
        StopAllCoroutines();
    }

    public override void Spawned()
    {
        AudioController.PlayClipAtPosition(_spawnClip, transform.position);
        _projectileStats = new ProjectileStats();
        _projectileStats.Damage = 1;
        StartCoroutine(AttackCor());
    }

    protected override void Move() { }
}