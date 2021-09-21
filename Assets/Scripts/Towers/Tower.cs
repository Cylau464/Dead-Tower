using UnityEngine;
using Spine.Unity;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform _defenderPoint;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    public Transform DefenderPoint => _defenderPoint;

    private GameObject _abilityPrefab;
    private TowerStats _stats;


    public void Init(TowerConfig config)
    {
        _abilityPrefab = config.AbilityPrefab;
        _stats = config.Stats;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
        //Vector3 weaponPos = _skeletonMecanim.skeleton.FindBone("weaponPoint");
        Weapon weapon = Instantiate(_weaponPrefab, _weaponPrefab.transform.position, Quaternion.identity, transform);
        weapon.Init(config.WeaponConfig);
    }
}