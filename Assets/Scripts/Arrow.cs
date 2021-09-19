using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float _stuckDepth = .5f;
    private Vector2 _lastVelocity;

    private void Update()
    {
        if (_hasHit == false)
        {
            float angle = Mathf.Atan2(_rigidBody.velocity.y, _rigidBody.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        _lastVelocity = _rigidBody.velocity;
    }

    public override void SetVelocity(Vector2 velocity)
    {
        _rigidBody.velocity = velocity;
    }

    protected override void HitToEnvironment(GameObject target)
    {
        StartCoroutine(Stuck(target));
    }

    protected override void HitToTarget(GameObject target)
    {
        if (target.TryGetComponent(out Enemy enemy))
        {
            enemy.GetHit(1);
        }
        else
        {
            Debug.LogError("Cant find Enemy component on " + target.name);
        }

        StartCoroutine(Stuck(target));
    }

    protected IEnumerator Stuck(GameObject target)
    {
        transform.parent = target.transform;
        _hasHit = true;
        _collider.enabled = false;
        float duration = _stuckDepth / _lastVelocity.magnitude;
        
        yield return new WaitForSeconds(duration);

        _rigidBody.velocity = Vector2.zero;
        _rigidBody.isKinematic = true;
    }
}