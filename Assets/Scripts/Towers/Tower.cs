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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _leverClip;
    [SerializeField] private AudioClip _gateOpenClip;
    [SerializeField] private AudioClip _takeDamageClip;

    public Transform DefenderPoint => _defenderPoint;

    private Weapon _weapon;
    public Weapon Weapon => _weapon;
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

    public void Init(TowerConfig config, TowerData data, bool inMenu)
    {
        _defensiveWeaponConfig = config.DefensiveWeaponConfig;
        _stats = data.Stats;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;

        if (inMenu == true)
            _defensiveWeaponButton.enabled = false;

        //Vector3 weaponPos = _skeletonMecanim.skeleton.FindBone("weaponPoint").GetWorldPosition(transform);
        Vector3 weaponPos = transform.position;
        weaponPos.z -= .01f; // for overlap collider
        _weapon = Instantiate(_weaponPrefab, weaponPos, Quaternion.identity, transform);
        _weapon.Init(config.WeaponConfig, inMenu);
        _weapon.OnAimStart += OnAimStart;
        _weapon.OnAimEnd += OnAimEnd;
    }

    public void TakeDamage(IDamageTaker attacker, int damage)
    {
        AudioController.PlayClipAtPosition(_takeDamageClip, transform.position);
        _stats.health = Mathf.Max(_stats.health - damage, -_stats.ExtraHealth);
        OnTakeDamage?.Invoke(_stats.Health);

        if(_stats.Health <= 0)
            Game.Instance.LevelEnd(false);

        _animator.SetTrigger(_takeDamageParamID);

        if(attacker != null)
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

        AudioController.PlayClipAtPosition(_gateOpenClip, transform.position);
        AudioController.PlayClipAtPosition(_leverClip, transform.position);
    }

    public void GateOpened()
    {
        DefensiveWeapon weapon = Instantiate(_defensiveWeaponConfig.Prefab, _defensiveWeaponSpawnPoint.position, Quaternion.identity);
        weapon.Init(_defensiveWeaponConfig, _stats.AbilityLevel);
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