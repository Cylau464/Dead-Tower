using UnityEngine;

public class ActiveDefensiveWeapon : DefensiveWeapon
{
    private Enemy _target;

    protected int _attackSpeedParamID;
    protected int _attackParamID;

    private new void Start()
    {
        base.Start();
        _attackSpeedParamID = Animator.StringToHash("attackSpeed");
        _attackParamID = Animator.StringToHash("attack");
        _animator.SetFloat(_attackSpeedParamID, 1f);
    }

    protected override void Attack(Enemy enemy)
    {
        _target = enemy;
        _rigidBody.velocity = Vector2.zero;
        _animator.SetTrigger(_attackParamID);
    }

    private void GiveDamage()
    {
        if(_target != null)
        {
            _target.TakeDamage(_stats.Damage);
            _animator.SetFloat(_attackSpeedParamID, _animator.GetFloat(_attackSpeedParamID) / 2f);
            Invoke(nameof(Move), .5f);
        }
    }

    public override void TakeDamage(int damage)
    {
        _stats.Health -= damage;

        if (_stats.Health <= 0)
            Dead();
    }
}