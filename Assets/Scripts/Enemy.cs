using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageTaker
{
    [SerializeField] protected SkeletonMecanim _skeletonMecanim;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Renderer _renderer;
    [SerializeField] protected Rigidbody2D _rigidBody;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private LayerMask _targetLayer;
    [Space]
    [SerializeField] protected AudioClip _spawnClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _deathClip;

    public KillTargets Type { get; private set; }
    protected EnemyStats _stats;
    public EnemyStats Stats => _stats;
    private int _maxHealth;
    private int _health;
    public float MoveSpeed { get; private set; }
    private Rewards _rewards;
    private bool _isDead;

    private int _speedParamID;
    protected int _attackParamID;
    private int _deadParamID;
    private int _takeDamageParamID;
    private int _grabParamID;
    private int _releaseParamID;

    private List<Projectile> _stuckProjectiles;

    public Action<Rewards, int> OnDead;
    public event EventHandler<float> HealthChanged;

    protected void Start()
    {
        _speedParamID = Animator.StringToHash("speed");
        _attackParamID = Animator.StringToHash("isAttack");
        _deadParamID = Animator.StringToHash("isDead");
        _takeDamageParamID = Animator.StringToHash("isTakeDamage");
        
        _stuckProjectiles = new List<Projectile>();

        Bounds bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
        _collider.offset = bounds.center;
        _collider.size = bounds.size;

        Game.Instance.OnLevelEnd += OnLevelEnd;
        Game.Instance.OnLevelContinued += OnLevelContinue;
    }

    private void LateUpdate()
    {
        float normalizedSpeed = _rigidBody.velocity.magnitude / MoveSpeed;
        _animator.SetFloat(_speedParamID, normalizedSpeed);
    }

    public bool TakeDamage(int damage)
    {
        if(damage < 0)
            throw new Exception("Attempt to deal negative damage");

        _health -= damage;
        HealthChanged?.Invoke(this, (float)_health / _maxHealth);
        AudioController.PlayClipAtPosition(_hitClip, transform.position);

        if (_health <= 0)
        {
            AudioController.PlayClipAtPosition(_deathClip, transform.position);
            Dead();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeDamage(int damage, Projectile projectile)
    {
        bool dead = TakeDamage(damage);
        
        if(dead == false)
            _stuckProjectiles.Add(projectile);

        return dead;
    }

    protected virtual void Attack()
    {
        StopMove();
        _animator.SetTrigger(_attackParamID);
    }

    protected virtual void Move()
    {
        _rigidBody.velocity = Vector2.left * MoveSpeed;
    }

    protected virtual void StopMove()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    private void Dead()
    {
        _isDead = true;
        OnDead?.Invoke(_rewards, _stats.Power);
        _animator.SetTrigger(_deadParamID);
        _rigidBody.simulated = false;
        _collider.enabled = false;
        this.enabled = false;
        _renderer.sortingOrder--;
        DailyQuestsHandler.Instance.EnemyDead(Type);

        foreach (Projectile projectile in _stuckProjectiles)
        {
            projectile.Unstuck(transform.position);
        }
    }

    public virtual void Init(EnemyConfig config)
    {
        Type = config.Type;
        _stats = config.Stats;
        int index = Random.Range(0, _stats.Health.Length);
        _health = _maxHealth = _stats.Health[index];
        MoveSpeed = _stats.MoveSpeed[index];
        _rewards = config.Rewards;
        gameObject.name = config.Name;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
    }

    public virtual void Spawned()
    {
        AudioController.PlayClipAtPosition(_spawnClip, transform.position);
        Move();
    }

    public void Grab()
    {
        _grabParamID = Animator.StringToHash("isGrab");
        _releaseParamID = Animator.StringToHash("isRelease");
        _animator.SetTrigger(_grabParamID);
        _rigidBody.simulated = false;
    }

    public void Release()
    {
        _animator.SetTrigger(_releaseParamID);
        _rigidBody.simulated = true;
    }

    public void GiveDamage()
    {
        if (_isDead == true) return;

        Vector2 checkPos = (Vector2)transform.position + Vector2.up * _renderer.bounds.size.y / 2f;
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

        bool targetIsDead = true;

        if (target != null)
        {
            if(target.TryGetComponent(out Tower tower))
                tower.TakeDamage(this, _stats.Damage);
            else if(target.TryGetComponent(out ActiveDefensiveWeapon defensiveWeapon))
                targetIsDead = defensiveWeapon.TakeDamage(_stats.Damage);
        }

        if (targetIsDead == true)
            Invoke(nameof(Move), .2f);
        else
            Invoke(nameof(Attack), .5f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if((1 << collision.gameObject.layer & _targetLayer) != 0)
        {
            Attack();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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

    private void OnLevelEnd(bool victory)
    {
        if (_isDead == true) return;

        if (victory == true)
            TakeDamage(int.MaxValue);
        else
            StopMove();
    }

    private void OnLevelContinue()
    {
        Move();
    }

    private void OnDestroy()
    {
        Game.Instance.OnLevelEnd -= OnLevelEnd;
        Game.Instance.OnLevelContinued -= OnLevelContinue;
    }
}
