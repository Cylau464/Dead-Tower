using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float _stuckDepth = .5f;
    [SerializeField] private float _dissolveDelay = 2f;
    [SerializeField] private float _dissolveDuration = 1f;
    private Material _material;
    private Vector2 _lastVelocity;
    private bool _isUnstucked;

    private void Awake()
    {
        _renderer.sortingLayerName = _towerSortingLayer;
        _renderer.sortingOrder = _towerSortingOrder;
        _rigidBody.isKinematic = true;
        _material = GetComponent<Renderer>().material;
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

    protected override void HitToEnvironment(GameObject target)
    {
        if (_hitParticlePrefab != null)
            Instantiate(_hitParticlePrefab, transform.position, Quaternion.identity);

        _hasHit = true;
        StartCoroutine(Stuck(target, true));
    }

    protected override void HitToTarget(GameObject target)
    {
        if (target.TryGetComponent(out IDamageTaker damageTaker))
        {
            if (_isUnstucked == true) return;

            _hasHit = true;

            if (damageTaker.TakeDamage(_stats.Damage, this) == false)
            {
                StartCoroutine(Stuck(target, false));

                if (_stats.ModifierConfig != null)
                    _stats.ModifierConfig.ApplyToTarget(target);
            }
        }
        else
        {
            Debug.LogError("Cant find IDamageTaker component on " + target.name);
        }            
    }

    protected IEnumerator Stuck(GameObject target, bool activateTimer)
    {
        CancelInvoke(nameof(SelfDestroy));

        if (activateTimer == true)
            StartCoroutine(Dissolve());

        transform.parent = target.transform;
        _collider.enabled = false;
        float duration = _stuckDepth / _lastVelocity.magnitude;
        
        yield return new WaitForSeconds(duration);

        _rigidBody.velocity = Vector2.zero;
        _rigidBody.simulated = false;
    }

    public override void Unstuck(Vector3 parentPosition)
    {
        StopAllCoroutines();
        transform.parent = null;
        Vector2 direction = (transform.position - parentPosition).normalized;
        _collider.enabled = true;
        _rigidBody.constraints = RigidbodyConstraints2D.None;
        _rigidBody.simulated = true;
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(direction * 6f, ForceMode2D.Impulse);
        _rigidBody.AddTorque(Random.Range(5f, 20f) * (Random.Range(0, 2) * 2 - 1), ForceMode2D.Impulse);
        _isUnstucked = true;
    }

    private IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(_dissolveDelay);

        float t = 1f;

        while(t > 0f)
        {
            t -= Time.deltaTime / _dissolveDuration;
            _material.SetFloat("_DissolveAmount", t);

            yield return null;
        }

        Destroy(gameObject);
    }
}