using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _targetLayer;

    private EnemyStats _stats;
    public EnemyStats Stats => _stats;
    private Rewards _rewards;

    private int _speedParamID;
    private int _attackParamID;
    private int _deadParamID;
    private int _takeDamageParamID;

    private List<Projectile> _stuckProjectiles;

    public Action<Rewards> OnDead;

    private void Start()
    {
        _speedParamID = Animator.StringToHash("speed");
        _attackParamID = Animator.StringToHash("isAttack");
        _deadParamID = Animator.StringToHash("isDead");
        _takeDamageParamID = Animator.StringToHash("isTakeDamage");
        _stuckProjectiles = new List<Projectile>();
    }

    private void LateUpdate()
    {
        float normalizedSpeed = _rigidBody.velocity.magnitude / _stats.MoveSpeed;
        _animator.SetFloat(_speedParamID, normalizedSpeed);
    }

    public bool TakeDamage(int damage)
    {
        if(damage < 0)
            throw new Exception("Attempt to deal negative damage");

        _stats.Health -= damage;

        if (_stats.Health <= 0)
        {
            Dead();
            return true;
        }
        else
        {
            //_animator.SetTrigger(_takeDamageParamID);
            return false;
        }
    }

    public bool TakeDamage(int damage, Projectile projectile)
    {
        _stuckProjectiles.Add(projectile);
        return TakeDamage(damage);
    }

    private void Attack()
    {
        _rigidBody.velocity = Vector2.zero;
        _animator.SetTrigger(_attackParamID);
    }

    private void Move()
    {
        _rigidBody.velocity = Vector2.left * _stats.MoveSpeed;
    }

    private void Dead()
    {
        OnDead?.Invoke(_rewards);
        _animator.SetTrigger(_deadParamID);
        _rigidBody.simulated = false;
        _collider.enabled = false;
        this.enabled = false;

        foreach(Projectile projectile in _stuckProjectiles)
        {
            projectile.Unstuck(transform.position);
        }
    }

    public virtual void Init(EnemyConfig config)
    {
        _stats = config.Stats;
        _rewards = config.Rewards;
        gameObject.name = config.Name;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        Spawned();
    }

    public void Spawned()
    {
        Move();
    }

    public void GiveDamage()
    {
        Vector2 checkPos = (Vector2)transform.position + (Vector2.up * _renderer.bounds.size.y / 2f);
        Vector2 checkSize = _collider.bounds.size;
        checkSize.x *= 1.5f;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, checkSize, 0f, _targetLayer);
        Collider2D target = null;
        float distanceToTarget = float.MinValue;

        foreach(Collider2D col in colliders)
        {
            float distance = Vector2.Distance(col.transform.position, transform.position);

            if (distanceToTarget < distance)
            {
                distanceToTarget = distance;
                target = col;
            }
        }

        if (target != null)
        {
            if(target.TryGetComponent(out Tower tower))
                tower.TakeDamage(this, _stats.Damage);
            else if(target.TryGetComponent(out ActiveDefensiveWeapon defensiveWeapon))
                defensiveWeapon.TakeDamage(_stats.Damage);
        }

        Invoke(nameof(Move), .2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((1 << collision.gameObject.layer & _targetLayer) != 0)
        {
            Attack();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Vector2 checkPos = (Vector2)transform.position + (Vector2.up * _renderer.bounds.size.y / 2f);
    //    Vector2 checkSize = _collider.bounds.size;
    //    checkSize.x *= 1.5f;
    //    Gizmos.DrawWireCube(checkPos, checkSize);
    //}
}
