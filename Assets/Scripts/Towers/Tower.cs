using UnityEngine;
using Spine.Unity;
using System;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform _defenderPoint;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    public Transform DefenderPoint => _defenderPoint;

    private Weapon _weapon;
    private GameObject _abilityPrefab;
    private TowerStats _stats;

    private int _openParamID;
    private int _closeParamID;
    private int _takeDamageParamID;
    private int _aimParamID;

    public Action<int> OnTakeDamage;

    private void Start()
    {
        _takeDamageParamID = Animator.StringToHash("takeDamage");
        _openParamID = Animator.StringToHash("open");
        _closeParamID = Animator.StringToHash("close");
        _aimParamID = Animator.StringToHash("isAim");
    }

    public void Init(TowerConfig config)
    {
        _abilityPrefab = config.AbilityPrefab;
        _stats = config.Stats;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;

        //Vector3 weaponPos = _skeletonMecanim.skeleton.FindBone("weaponPoint").GetWorldPosition(transform);
        Vector3 weaponPos = transform.position;
        weaponPos.z -= .01f; // for overlap collider
        _weapon = Instantiate(_weaponPrefab, weaponPos, Quaternion.identity, transform);
        _weapon.Init(config.WeaponConfig);
        _weapon.OnAimStart += OnAimStart;
        _weapon.OnAimEnd += OnAimEnd;
    }

    public void TakeDamage(Enemy attacker, int damage)
    {
        _stats.Health = Mathf.Max(_stats.Health - damage, 0);
        OnTakeDamage?.Invoke(_stats.Health);
        _animator.SetTrigger(_takeDamageParamID);
        attacker.TakeDamage(int.MaxValue);
    }

    private void OnAimStart()
    {
        _animator.SetBool(_aimParamID, true);
    }

    private void OnAimEnd()
    {
        _animator.SetBool(_aimParamID, false);
    }

    private void OnDestroy()
    {
        if(_weapon != null)
        {
            _weapon.OnAimStart -= OnAimStart;
            _weapon.OnAimEnd -= OnAimEnd;
        }
    }
}