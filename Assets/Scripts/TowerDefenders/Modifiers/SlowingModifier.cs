using UnityEngine;

public class SlowingModifier : Modifier
{
    private Rigidbody2D _targetRigidbody;
    private Enemy _enemy;

    private void Start()
    {
        _targetRigidbody = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }

    protected override void ApplyEffect()
    {
        if (_targetRigidbody.velocity.magnitude > 0f)
            _targetRigidbody.velocity = _targetRigidbody.velocity.normalized * (_enemy.MoveSpeed / _value);
    }

    protected override void CancelEffect()
    {
        if (_targetRigidbody.velocity.magnitude > 0f)
            _targetRigidbody.velocity = _targetRigidbody.velocity.normalized * _enemy.MoveSpeed;
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