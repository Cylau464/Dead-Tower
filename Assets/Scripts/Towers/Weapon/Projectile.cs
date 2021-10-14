using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidBody = null;
    [SerializeField] protected Collider2D _collider = null;
    [SerializeField] protected Renderer _renderer;
    public Renderer Renderer => _renderer;

    [Space]
    [SerializeField] protected LayerMask _targetLayer = 0;
    [SerializeField] private LayerMask _environmentLayer = 0;
    [SerializeField] protected string _towerSortingLayer;
    [SerializeField] protected string _projectileSortingLayer;
    [SerializeField] protected int _towerSortingOrder;
    [SerializeField] protected GameObject _hitParticlePrefab;
    protected bool _hasHit;
    protected ProjectileStats _stats;

    [Space]
    [SerializeField] private float _lifetime = 10f;
    [Header("Audio Clips")]
    [SerializeField] protected AudioClip _hitClip;

    public Action OnShot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1 << collision.gameObject.layer & _targetLayer) != 0)
        {
            HitToTarget(collision.gameObject);
        }
        else if ((1 << collision.gameObject.layer & _environmentLayer) != 0)
        {
            HitToEnvironment(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & _targetLayer) == 0)
        {
            if (_renderer.sortingLayerName != _projectileSortingLayer)
                ChangeSortLayer();
        }
    }

    protected void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void Init(ProjectileStats stats)
    {
        _stats = stats;
    }

    public virtual void Launch(Vector2 velocity)
    {
        Invoke(nameof(SelfDestroy), _lifetime);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = velocity;
        OnShot?.Invoke();
    }

    private void ChangeSortLayer()
    {
        _renderer.sortingLayerName = _projectileSortingLayer;
        _renderer.sortingOrder = 0;
    }

    public abstract void Unstuck(Vector3 parentPosition);
    protected abstract void HitToTarget(GameObject target);
    protected abstract void HitToEnvironment(GameObject target);
}
