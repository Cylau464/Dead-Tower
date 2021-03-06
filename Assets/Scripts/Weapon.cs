using UnityEngine;
using Spine.Unity;
using System;
using System.Linq;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;

    private float _charge;
    private bool _isCharging;

    [SerializeField] private TrajectoryLine _line = null;
    private ProjectileConfig _projectileConfig;
    private Projectile _curProjectile;
    private Spine.Bone _projectileBone;
    private Spine.Bone _shootPointBone;
    private Tower _tower;
    private WeaponStats _stats;
    private WeaponConfig _config;
    public WeaponConfig Config => _config;

    private AudioSource _chargeAudio;
    private Coroutine _vibrateCor;

    // Anim params
    private int _aimingParamID;
    private int _shootParamID;
    private int _takeDamageParamID;

    public Action OnAimStart;
    public Action OnAimEnd;
    public Action OnProjectilesEnd;

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
            ProjectileStats pStats = new ProjectileStats(_tower.Stats.Damage, null);
            _projectileConfig.Stats = pStats;
        }
        else
        {
            throw new Exception("Cant find a tower in parent object");
        }

        _line.SetYLimit(_shootPointBone.GetWorldPosition(transform));

        Game.Instance.OnLevelEnd += OnLevelEnd;
        Game.Instance.OnLevelContinued += OnLevelContinue;
        Vibration.Init();
    } 

    public void Init(WeaponConfig config, bool inMenu)
    {
        _config = config;
        _projectileConfig = config.ProjectileConfig;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
        _stats = config.Stats;

        if (inMenu == true)
            _collider.enabled = false;
    }

    private void Update()
    {
        if(_curProjectile != null)
        {
            _curProjectile.transform.position = _projectileBone.GetWorldPosition(transform);

            if(_isCharging == true)
            {
                bool lineTextureOffset = false;
                _charge = Mathf.Min(_charge + _stats.ChargeSpeed * Time.deltaTime, _stats.MaxCharge);

                if (_charge >= _stats.MaxCharge)
                    lineTextureOffset = true;

                _line.UpdateLine(_charge * _curProjectile.transform.right, _shootPointBone.GetWorldPosition(transform), lineTextureOffset);

                if (_vibrateCor != null && _charge >= _stats.MaxCharge)
                    StopCoroutine(_vibrateCor);
            }
        }
        else if (_vibrateCor != null)
        {
            StopCoroutine(_vibrateCor);
        }
    }

    private void StartCharge()
    {
        if(SLS.Data.Game.ProjectilesCount.Value[_projectileConfig.Index] <= 0)
        {
            OnProjectilesEnd?.Invoke();
            return;
        }

        if (_curProjectile != null)
            return;

        if(_config.ChargeClip != null)
            _chargeAudio = AudioController.PlayClipAtPosition(_config.ChargeClip, transform.position);

        Vector2 pos = _projectileBone.GetWorldPosition(transform);
        Quaternion rot = Quaternion.Euler(0f, 0f, _projectileBone.Rotation);
        _curProjectile = Instantiate(_projectileConfig.Prefab, pos, rot).GetComponent<Projectile>();
        _curProjectile.Init(_projectileConfig.Stats);

        _isCharging = true;
        _charge = _stats.MinCharge;
        _line.gameObject.SetActive(true);
        _line.UpdateLine(_charge * _curProjectile.transform.right, _shootPointBone.GetWorldPosition(transform));
        _animator.SetTrigger(_aimingParamID);
        OnAimStart?.Invoke();

        _vibrateCor = StartCoroutine(Vibrate());
    }

    private void Shoot()
    {
        if (_curProjectile == null) return;

        if (_chargeAudio != null && _chargeAudio.isPlaying == true)
            _chargeAudio.Stop();

        AudioController.PlayClipAtPosition(_config.ShotClip, transform.position);

        _isCharging = false;
        _line.gameObject.SetActive(false);
        _animator.SetTrigger(_shootParamID);
        OnAimEnd?.Invoke();

        if (_vibrateCor != null)
            StopCoroutine(_vibrateCor);
    }

    public void LaunchProjectile()
    {
        _curProjectile.Launch(_curProjectile.transform.right * _charge);
        _curProjectile = null;
        int[] projectilesCount = SLS.Data.Game.ProjectilesCount.Value.ToArray();
        projectilesCount[_projectileConfig.Index]--;
        SLS.Data.Game.ProjectilesCount.Value = projectilesCount;
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

    private void OnLevelEnd(bool victory)
    {
        _collider.enabled = false;
    }

    private void OnLevelContinue()
    {
        _collider.enabled = true;
    }

    private IEnumerator Vibrate()
    {
        long time;
        long[] pattern;
        float t;

        while (true)
        {
            t = (_charge - _stats.MinCharge) / (_stats.MaxCharge - _stats.MinCharge);
            time = (long)Mathf.Lerp(100, 10, t);
            pattern = new long[] { 0, time };

            if(SLS.Data.Settings.VibrationEnabled.Value == true)
                Vibration.Vibrate(pattern, -1);
            
            yield return new WaitForSeconds((float)(time * 2f) / 1000);
        }
    }

    private void OnDestroy()
    {
        if(_tower != null)
            _tower.OnTakeDamage -= TakeDamage;

        Game.Instance.OnLevelEnd -= OnLevelEnd;
        Game.Instance.OnLevelContinued -= OnLevelContinue;
    }
}