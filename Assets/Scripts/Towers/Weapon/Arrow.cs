using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float _stuckDepth = .5f;
    private Vector2 _lastVelocity;
    private bool _isUnstucked;

    private void Awake()
    {
        _renderer.sortingLayerName = _towerSortingLayer;
        _renderer.sortingOrder = 10;
        _rigidBody.isKinematic = true;
    }

    private void Update()
    {
        if (_rigidBody.isKinematic == false && _hasHit == false)
        {
            float angle = Mathf.Atan2(_rigidBody.velocity.y, _rigidBody.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        _lastVelocity = _rigidBody.velocity;
    }

    public override void Launch(Vector2 velocity)
    {
        base.Launch(velocity);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = velocity;
        Invoke(nameof(SetSortLayer), .5f);
    }

    private void SetSortLayer()
    {
        _renderer.sortingLayerName = _projectileSortingLayer;
        _renderer.sortingOrder = 0;
    }

    protected override void HitToEnvironment(GameObject target)
    {
        StartCoroutine(Stuck(target, true));
    }

    protected override void HitToTarget(GameObject target)
    {
        if (target.TryGetComponent(out Enemy enemy))
        {
            if (_isUnstucked == true) return;

            if(enemy.TakeDamage(1, this) == false)
                StartCoroutine(Stuck(target, false));
        }
        else
        {
            Debug.LogError("Cant find Enemy component on " + target.name);
        }            
    }

    protected IEnumerator Stuck(GameObject target, bool activateTimer)
    {
        CancelInvoke(nameof(SelfDestroy));

        if (activateTimer == true)
            Destroy(gameObject, 2f);

        _hasHit = true;
        _collider.enabled = false;
        float duration = _stuckDepth / _lastVelocity.magnitude;
        
        yield return new WaitForSeconds(duration);

        _rigidBody.velocity = Vector2.zero;
        _rigidBody.simulated = false;
    }

    public override void Unstuck(Vector3 parentPosition)
    {
        transform.parent = null;
        Vector2 direction = (transform.position - parentPosition).normalized;
        _collider.enabled = true;
        _rigidBody.constraints = RigidbodyConstraints2D.None;
        _rigidBody.simulated = true;
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(direction * 2f, ForceMode2D.Impulse);
        _rigidBody.AddTorque(Random.Range(-90f, 90f));
        _isUnstucked = true;
    }
}