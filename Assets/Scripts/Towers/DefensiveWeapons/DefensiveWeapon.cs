using UnityEngine;
using Spine.Unity;

public abstract class DefensiveWeapon : MonoBehaviour
{
    [SerializeField] protected SkeletonMecanim _skeletonMecanim;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _rigidBody;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected LayerMask _targetLayer;
    protected DefensiveWeaponStats _stats;

    protected int _speedParamID;
    protected int _deadParamID;

    private void Start()
    {
        _speedParamID = Animator.StringToHash("speed");
        _deadParamID = Animator.StringToHash("dead");
    }

    private void LateUpdate()
    {
        float normalizedSpeed = _rigidBody.velocity.magnitude / _stats.MoveSpeed;
        _animator.SetFloat(_speedParamID, normalizedSpeed);
    }

    public void Init(DefensiveWeaponConfig config)
    {
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
        _stats = config.Stats;
        Move();
    }

    protected void Move()
    {
        _rigidBody.velocity = Vector2.right * _stats.MoveSpeed;
    }

    protected void Dead()
    {
        _rigidBody.simulated = false;
        _collider.enabled = false;
        _animator.SetTrigger(_deadParamID);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & _targetLayer) != 0)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
                Attack(enemy);
        }
    }

    protected abstract void Attack(Enemy enemy);
    public abstract void TakeDamage(int damage);
}