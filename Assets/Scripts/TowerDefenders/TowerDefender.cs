﻿using System.Collections;
using UnityEngine;
using Spine.Unity;

public class TowerDefender : MonoBehaviour, IDamageTaker
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _minAttackDistance = 3f;
    [SerializeField] private float _maxAttackDistance = 5f;
    [SerializeField] private float _projectileFlightDuration = 1f;
    [SerializeField] private float _shotCooldown = 1f;
    private float _curShotCooldown;
    private Vector2 _aimPosition;

    private Projectile _projectilePrefab;
    private ModifierConfig _modifierConfig;
    private TowerDefenderStats _stats;

    private int _attackParamID;
    private int _deadParamID;
    private int _respawnParamID;
    private int _victoryParamID;
    private int _defeatParamID;

    private Projectile _projectile;
    private Rigidbody2D _target;
    private Spine.Bone _handBone;

    private bool _isDead;
    public bool IsDead => _isDead;

    public static TowerDefender Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _attackParamID = Animator.StringToHash("attack");
        _deadParamID = Animator.StringToHash("dead");
        _respawnParamID = Animator.StringToHash("respawn");
        _victoryParamID = Animator.StringToHash("victory");
        _defeatParamID = Animator.StringToHash("defeat");

        _handBone = _skeletonMecanim.skeleton.FindBone("arrow");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, _groundLayer);

        if(hit == true)
        {
            _aimPosition = hit.point + Vector2.up * .25f;
        }
    }

    private void Update()
    {
        if(_isDead == false && _curShotCooldown <= Time.time)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            float shootDistance = _maxAttackDistance + _stats.ShootDistance;
            Debug.DrawRay(_aimPosition, Vector2.right * shootDistance, Color.cyan, 1f);
            Debug.DrawRay(_aimPosition, Vector2.right * _minAttackDistance, Color.red, 1f);

            if (Physics2D.RaycastNonAlloc(_aimPosition, Vector2.right, hits, shootDistance, _targetLayer) > 0)
            {
                //hits.OrderBy(x => x.distance);
                _target = null;

                foreach(RaycastHit2D hit in hits)
                {
                    if (hit.collider == null) break;

                    if (_modifierConfig.HasModifier(hit.collider.gameObject) == false
                        && Mathf.Abs(hit.collider.transform.position.x - _aimPosition.x) > _minAttackDistance)
                    {
                        _target = hit.collider.GetComponent<Rigidbody2D>();
                        break;
                    }
                }

                if (_target == null) return;
                    //target = hits[0].collider.GetComponent<Rigidbody2D>();

                _animator.SetTrigger(_attackParamID);
                _curShotCooldown = Time.time + _shotCooldown;
            }
        }

        if(_projectile != null)
        {
            _projectile.transform.rotation = Quaternion.Euler(0f, 0f, _handBone.Rotation);
            Vector3 pos = _handBone.GetWorldPosition(transform) + _projectile.transform.right * _projectile.Renderer.bounds.size.x / 3f;
            _projectile.transform.position = pos;
        }
    }

    public void Init(TowerDefenderConfig config)
    {
        _projectilePrefab = config.ProjectilePrefab;
        _modifierConfig = config.Modifier;
        _stats = config.Stats;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
    }

    public bool TakeDamage(int damage)
    {
        _stats.Health -= damage;

        if (_stats.Health <= 0)
            Dead();

        return _stats.Health <= 0;
    }

    public bool TakeDamage(int damage, Projectile projectile)
    {
        return TakeDamage(damage);
    }

    private void Dead()
    {
        _isDead = true;
        _animator.SetTrigger(_deadParamID);
    }

    public void PullTheArrow()
    {
        _projectile = Instantiate(_projectilePrefab, _handBone.GetWorldPosition(transform), Quaternion.Euler(0f, 0f, _handBone.Rotation));
        ProjectileStats pStats = new ProjectileStats(_stats.Damage, _modifierConfig);
        _projectile.Init(pStats);
    }

    public void Shoot()
    {
        Vector2 boundsSize = _target.GetComponent<Renderer>().bounds.size;
        Vector3 predictTargetPos = _target.position + _target.velocity * _projectileFlightDuration;
        predictTargetPos.y += boundsSize.y;
        Vector3 velocity = Ballistik.CalculateBestThrowSpeed(_projectile.transform.position, predictTargetPos, _projectileFlightDuration);
        _projectile.transform.rotation = Quaternion.Euler(velocity);
        _projectile.Launch(velocity);
        _projectile = null;
    }
}