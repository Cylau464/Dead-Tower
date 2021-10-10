using UnityEngine;
using Spine.Unity;
using System.Collections;

public class Archer : Enemy
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _minDistanceToShoot = 5f;
    [SerializeField] private float _attackCooldown = 3f;
    [SerializeField] private float _projectileFlightDuration = 1f;
    [SerializeField] private float _rotateToTargetDuration = .5f;

    private float _curAttackCooldown;
    private bool _isShoot;
    private TowerDefender _target;
    private Vector2 _targetBoundsSize;
    private Projectile _projectile;
    private Spine.Bone _arrowBone;
    private Spine.Bone _headBone;
    private Spine.Bone _bowBone;

    private Vector3 _rotateDirection;
    private float _rotateAngle;
    private int _shootParamID;

    private new void Start()
    {
        base.Start();
        _shootParamID = Animator.StringToHash("isShoot");
    }

    private void Update()
    {
        if (_curAttackCooldown <= Time.time && _isShoot == false)
        {
            Vector3 targetPos = _target.transform.position;
            targetPos.y = transform.position.y;
            float distanceToTarget = Vector3.Distance(targetPos, transform.position);

            if (distanceToTarget >= _minDistanceToShoot)
                Shoot();
        }

        if (_projectile != null)
        {
            _projectile.transform.rotation = Quaternion.Euler(0f, 0f, _arrowBone.LocalToWorldRotation(_arrowBone.Rotation));
            Vector3 pos = _arrowBone.GetWorldPosition(transform) + _projectile.transform.right * _projectile.Renderer.bounds.size.x / 3f;
            _projectile.transform.position = pos;
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        _target = TowerDefender.Instance;
        _targetBoundsSize = _target.GetComponent<Renderer>().bounds.size;
        _curAttackCooldown = Time.time + _attackCooldown;
        _arrowBone = _skeletonMecanim.skeleton.FindBone("arrow");
        _bowBone = _skeletonMecanim.skeleton.FindBone("vector");
        _headBone = _skeletonMecanim.skeleton.FindBone("head");
    }

    private void Shoot()
    {
        if (_target.IsDead == true) return;

        StartCoroutine(RotateToTarget());
    }

    public void PullTheArrow()
    {
        _projectile = Instantiate(_projectilePrefab, _arrowBone.GetWorldPosition(transform), Quaternion.Euler(0f, 0f, _arrowBone.Rotation));
        ProjectileStats pStats = new ProjectileStats(_stats.Damage, null);
        _projectile.Init(pStats);
    }

    public void LaunchProjectile()
    {
        _curAttackCooldown = Time.time + _attackCooldown;

        Vector3 offset = Vector2.up * _renderer.bounds.size.y / Random.Range(1.5f, 3f);
        Vector3 targetPos = _target.transform.position + offset;
        Vector3 velocity = Ballistik.CalculateBestThrowSpeed(_projectile.transform.position, targetPos, _projectileFlightDuration);
        _projectile.transform.rotation = Quaternion.Euler(velocity);
        _projectile.Launch(velocity);
        _projectile = null;
        _isShoot = false;
    }

    private IEnumerator RotateToTarget()
    {
        _isShoot = true;
        _rigidBody.velocity = Vector2.zero;

        Vector3 offset = Vector2.up * _renderer.bounds.size.y / Random.Range(1.5f, 3f);
        Vector3 targetPos = _target.transform.position + offset;
        Vector3 vec = _bowBone.GetWorldPosition(transform) - targetPos;
        float targetRot = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        targetRot = _bowBone.Rotation - targetRot;
        float startRot = _bowBone.Rotation;
        float t = 0f;

        while(t < 1f)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / _rotateToTargetDuration);
            _bowBone.Rotation = Mathf.Lerp(startRot, targetRot, t);

            yield return new WaitForEndOfFrame();
        }

        _animator.SetTrigger(_shootParamID);

        while (_isShoot == true)
            yield return new WaitForEndOfFrame();

        t = 0f;

        while (t < 1f)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / _rotateToTargetDuration);
            _bowBone.Rotation = Mathf.Lerp(targetRot, startRot, t);

            yield return new WaitForEndOfFrame();
        }

        Move();
    }

    protected override void OnCollisionEnter2D(Collision2D collision) { }
}