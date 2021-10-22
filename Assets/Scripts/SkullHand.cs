using UnityEngine;
using Spine.Unity;

public class SkullHand : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim;

    private Spine.Bone _bone;
    private Enemy _enemy;
    private Vector2 _boundsSize;

    private void Start()
    {
        _bone = _skeletonMecanim.skeleton.FindBone("pivot_point");
    }

    private void Update()
    {
        if (_enemy != null)
            _enemy.transform.position = _bone.GetWorldPosition(transform) - Vector3.up * _boundsSize.y;
    }

    public void SetEnemy(Enemy enemy)
    {
        _enemy = enemy;
        _boundsSize = enemy.GetComponent<Renderer>().bounds.size;
    }

    public void Release()
    {
        _enemy.Release();
        _enemy = null;
    }
}
