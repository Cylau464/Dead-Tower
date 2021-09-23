using UnityEngine;
using Spine.Unity;
using UnityEngine.EventSystems;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _minCharge = 4f;
    [SerializeField] private float _maxCharge = 16f;
    [SerializeField] private float _chargeSpeed = 1f;
    private float _charge;
    private bool _isCharging;

    [SerializeField] private TrajectoryLine _line = null;
    private GameObject _projectilePrefab;
    private Projectile _curProjectile;
    private Spine.Bone _projectileBone;
    private Spine.Bone _shootPointBone;
    private Tower _tower;

    // Anim params
    private int _aimingParamID;
    private int _shootParamID;
    private int _takeDamageParamID;

    public Action OnAimStart;
    public Action OnAimEnd;

    private void Start()
    {
        _aimingParamID = Animator.StringToHash("isAim");
        _shootParamID = Animator.StringToHash("shoot");
        _takeDamageParamID = Animator.StringToHash("takeDamage");
        _projectileBone = _skeletonMecanim.skeleton.FindBone("projectile_point");
        _shootPointBone = _skeletonMecanim.skeleton.FindBone("projectile_point2");

        if (transform.parent.TryGetComponent(out Tower tower))
        {
            _tower = tower;
            _tower.OnTakeDamage += TakeDamage;
        }
        else
        {
            throw new Exception("Cant find a tower in parent object");
        }

        _line.SetYLimit(_shootPointBone.GetWorldPosition(transform));
    } 

    private void Update()
    {
        if(_curProjectile != null)
        {
            _curProjectile.transform.position = _projectileBone.GetWorldPosition(transform);

            if(_isCharging == true)
            {
                bool lineTextureOffset = false;
                _charge = Mathf.Min(_charge + _chargeSpeed * Time.deltaTime, _maxCharge);


                if (_charge >= _maxCharge)
                    lineTextureOffset = true;

                _line.UpdateLine(_charge * _curProjectile.transform.right, _shootPointBone.GetWorldPosition(transform), lineTextureOffset);

            }
        }
    }

    private void StartCharge()
    {
        if (_curProjectile != null)
            return;

        Vector2 pos = _projectileBone.GetWorldPosition(transform);
        Quaternion rot = Quaternion.Euler(0f, 0f, _projectileBone.Rotation);
        _curProjectile = Instantiate(_projectilePrefab, pos, rot).GetComponent<Projectile>();

        _isCharging = true;
        _charge = _minCharge;
        _line.gameObject.SetActive(true);
        _line.UpdateLine(_charge * _curProjectile.transform.right, _shootPointBone.GetWorldPosition(transform));
        _animator.SetTrigger(_aimingParamID);
        OnAimStart?.Invoke();
    }

    private void Shoot()
    {
        if (_curProjectile == null) return;

        _isCharging = false;
        _line.gameObject.SetActive(false);
        _animator.SetTrigger(_shootParamID);
        OnAimEnd?.Invoke();
    }

    public void LaunchProjectile()
    {
        _curProjectile.Launch(_curProjectile.transform.right * _charge);
        _curProjectile = null;
    }

    public void Init(WeaponConfig config)
    {
        _projectilePrefab = config.ProjectilePrefab;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
    }

    private void TakeDamage(int healthLeft)
    {
        _animator.SetTrigger(_takeDamageParamID);
    }

    private void OnMouseDown()
    {
        StartCharge();
    }

    private void OnMouseUp()
    {
        Shoot();
    }

    private void OnMouseDrag()
    {
        StartCharge();
    }

    private void OnDestroy()
    {
        if(_tower != null)
            _tower.OnTakeDamage -= TakeDamage;
    }
}