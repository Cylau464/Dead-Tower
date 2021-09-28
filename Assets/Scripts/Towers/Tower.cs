using UnityEngine;
using Spine.Unity;
using System;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform _defenderPoint;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _defensiveWeaponButton;
    [SerializeField] private Transform _defensiveWeaponSpawnPoint;
    [SerializeField] private LayerMask _defensiveWeaponLayer;

    public Transform DefenderPoint => _defenderPoint;

    private Weapon _weapon;
    private DefensiveWeaponConfig _defensiveWeaponConfig;
    private bool _defensiveWeaponActivated;
    private TowerStats _stats;
    public TowerStats Stats => _stats;

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
        _defensiveWeaponButton.onClick.AddListener(OpenGate);
    }

    public void Init(TowerConfig config)
    {
        _defensiveWeaponConfig = config.DefensiveWeaponConfig;
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

    private void OpenGate()
    {
        if (_defensiveWeaponActivated == true) return;

        _animator.SetTrigger(_openParamID);
        _defensiveWeaponButton.interactable = false;
        _defensiveWeaponActivated = true;
        _defensiveWeaponButton.GetComponent<SkeletonMecanim>().skeleton.SetColor(Color.gray);
    }

    public void GateOpened()
    {
        DefensiveWeapon weapon = Instantiate(_defensiveWeaponConfig.Prefab, _defensiveWeaponSpawnPoint.position, Quaternion.identity);
        _defensiveWeaponConfig.Stats.Damage *= _stats.AbilityLevel;
        weapon.Init(_defensiveWeaponConfig);
    }

    private void CloseGate()
    {
        _animator.SetTrigger(_closeParamID);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if((1 << collision.gameObject.layer & _defensiveWeaponLayer) != 0)
        {
            CloseGate();
        }
    }
}