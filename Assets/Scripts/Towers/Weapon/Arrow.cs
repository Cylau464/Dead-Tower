using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float _stuckDepth = .5f;
    private Vector2 _lastVelocity;

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
        StartCoroutine(Stuck());
    }

    protected override void HitToTarget(GameObject target)
    {
        if (target.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(1);
        }
        else
        {
            Debug.LogError("Cant find Enemy component on " + target.name);
        }

        StartCoroutine(Stuck(target));
    }

    protected IEnumerator Stuck(GameObject target = null)
    {
        if(target != null)
            transform.parent = target.transform;

        _hasHit = true;
        _collider.enabled = false;
        float duration = _stuckDepth / _lastVelocity.magnitude;
        
        yield return new WaitForSeconds(duration);

        _rigidBody.velocity = Vector2.zero;
        _rigidBody.isKinematic = true;
    }
}