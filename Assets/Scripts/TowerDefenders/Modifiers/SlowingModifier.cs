using UnityEngine;

public class SlowingModifier : Modifier
{
    private Rigidbody2D _targetRigidbody;
    private EnemyStats _targetStats;

    private void Start()
    {
        _targetRigidbody = GetComponent<Rigidbody2D>();
        _targetStats = GetComponent<Enemy>().Stats;
    }

    protected override void ApplyEffect()
    {
        if (_targetRigidbody.velocity.magnitude > 0f)
            _targetRigidbody.velocity = _targetRigidbody.velocity.normalized * (_targetStats.MoveSpeed / _value);
    }

    protected override void CancelEffect()
    {
        if (_targetRigidbody.velocity.magnitude > 0f)
            _targetRigidbody.velocity = _targetRigidbody.velocity.normalized * _targetStats.MoveSpeed;
    }

    protected override void Timer()
    {
        _duration -= Time.deltaTime;

        if (_duration <= 0f)
        {
            Destroy(this);
            return;
        }

        ApplyEffect();
    }
}