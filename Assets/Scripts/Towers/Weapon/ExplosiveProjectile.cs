using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private float _rotationMultiplier = 1f;

    private void Awake()
    {
        _renderer.sortingLayerName = _towerSortingLayer;
        _renderer.sortingOrder = _towerSortingOrder;
        _rigidBody.isKinematic = true;
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, _rigidBody.velocity.magnitude * _rotationMultiplier * Time.deltaTime);
    }

    private void Explode()
    {
        if (_hitParticlePrefab != null)
            Instantiate(_hitParticlePrefab, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _targetLayer);

        foreach (Collider2D col in colliders)
        {
            if (col.TryGetComponent(out Enemy enemy) == true)
                enemy.TakeDamage(_stats.Damage);
        }

        Destroy(gameObject);
    }

    protected override void HitToEnvironment(GameObject target)
    {
        Explode();
    }

    protected override void HitToTarget(GameObject target)
    {
        Explode();
    }

    public override void Unstuck(Vector3 parentPosition) { }
}