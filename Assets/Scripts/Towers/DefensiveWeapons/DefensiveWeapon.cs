using UnityEngine;
using Spine.Unity;
using System;
using System.Collections.Generic;

public abstract class DefensiveWeapon : MonoBehaviour, IDamageTaker
{
    [SerializeField] protected SkeletonMecanim _skeletonMecanim;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _rigidBody;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected LayerMask _targetLayer;
    protected DefensiveWeaponStats _stats;

    protected int _speedParamID;
    protected int _deadParamID;

    protected bool _isDead;
    protected int _maxHealth;

    protected List<Projectile> _stuckProjectiles;

    public event EventHandler<float> HealthChanged;

    protected void Start()
    {
        _speedParamID = Animator.StringToHash("speed");
        _deadParamID = Animator.StringToHash("dead");
        _stuckProjectiles = new List<Projectile>();
    }

    private void LateUpdate()
    {
        float normalizedSpeed = _rigidBody.velocity.magnitude / _stats.MoveSpeed;
        _animator.SetFloat(_speedParamID, normalizedSpeed);
    }

    public void Init(DefensiveWeaponConfig config, int level)
    {
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
        _stats = config.Stats;
        _stats.Health *= level;
        _maxHealth = _stats.Health;
        Move();
    }

    protected void Move()
    {
        _rigidBody.velocity = Vector2.right * _stats.MoveSpeed;
    }

    protected void Dead()
    {
        _isDead = true;
        _rigidBody.simulated = false;
        _collider.enabled = false;
        _animator.SetTrigger(_deadParamID);

        foreach (Projectile projectile in _stuckProjectiles)
        {
            projectile.Unstuck(transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & _targetLayer) != 0)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
                Attack(enemy);
        }
    }

    protected void OnHealthChange()
    {
        HealthChanged?.Invoke(this, (float) _stats.Health / _maxHealth);
    }

    protected abstract void Attack(Enemy enemy);
    public abstract bool TakeDamage(int damage);
    public abstract bool TakeDamage(int damage, Projectile projectile);
}