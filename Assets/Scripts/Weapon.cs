using UnityEngine;
using Spine.Unity;
using UnityEngine.EventSystems;

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
    [SerializeField] private Transform _shootPoint = null;
    private GameObject _projectilePrefab;

    private void Update()
    {
        if(_isCharging == true)
        {
            _charge += _chargeSpeed * Time.deltaTime;
            _line.UpdateLine(_charge * _shootPoint.right, _shootPoint.position);
        }
    }

    private void StartCharge()
    {
        _isCharging = true;
        _charge = _minCharge;
        _line.gameObject.SetActive(true);
        _line.UpdateLine(_charge * _shootPoint.right, _shootPoint.position);
    }
    private void Shoot()
    {
        _isCharging = false;
        GameObject projectile = Instantiate(_projectilePrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.GetComponent<Projectile>().SetVelocity(_shootPoint.right * _charge);
        _line.gameObject.SetActive(false);
    }

    public void Init(WeaponConfig config)
    {
        _projectilePrefab = config.ProjectilePrefab;
        //_skeletonMecanim.skeletonDataAsset = config.Skeleton;
        //_skeletonMecanim.Initialize(true);
        //_animator.runtimeAnimatorController = config.AnimatorController;
    }

    private void OnMouseDown()
    {
        StartCharge();
    }

    private void OnMouseUp()
    {
        Shoot();
    }
}