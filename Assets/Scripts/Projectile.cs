using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidBody = null;
    [SerializeField] protected Collider2D _collider = null;
    
    [Space]
    [SerializeField] private LayerMask _targetLayer = 0;
    [SerializeField] private LayerMask _environmentLayer = 0;
    protected bool _hasHit;

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

    protected abstract void HitToTarget(GameObject target);
    protected abstract void HitToEnvironment(GameObject target);
    public abstract void SetVelocity(Vector2 velocity);
}
