using UnityEngine;

public class ProjectileMask : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    private float _prevParentRot;

    private void Start()
    {
        _projectile.OnShot += Hide;
        _prevParentRot = transform.parent.rotation.eulerAngles.z;
    }

    private void Update()
    {
        float rot = transform.parent.rotation.eulerAngles.z - _prevParentRot;
        transform.Rotate(Vector3.forward, -rot);

        _prevParentRot = transform.parent.rotation.eulerAngles.z;
    }

    private void Hide()
    {
        _projectile.OnShot -= Hide;
        gameObject.SetActive(false);
    }
}