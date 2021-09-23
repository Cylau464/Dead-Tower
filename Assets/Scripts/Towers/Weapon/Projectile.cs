using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidBody = null;
    [SerializeField] protected Collider2D _collider = null;
    [SerializeField] protected Renderer _renderer;

    [Space]
    [SerializeField] private LayerMask _targetLayer = 0;
    [SerializeField] private LayerMask _environmentLayer = 0;
    [SerializeField] protected string _towerSortingLayer;
    [SerializeField] protected string _projectileSortingLayer;
    protected bool _hasHit;

    [Space]
    [SerializeField] private float _lifetime = 10f;

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

    protected void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public virtual void Launch(Vector2 velocity)
    {
        Invoke(nameof(SelfDestroy), _lifetime);
    }

    public abstract void Unstuck(Vector3 parentPosition);
    protected abstract void HitToTarget(GameObject target);
    protected abstract void HitToEnvironment(GameObject target);
}
