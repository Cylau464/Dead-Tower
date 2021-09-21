using System.Collections;
using UnityEngine;
using Spine.Unity;

public class TowerDefender : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim _skeletonMecanim = null;
    [SerializeField] private Animator _animator = null;

    private GameObject _projectilePrefab;
    private ModifierConfig _modifierConfig;
    private TowerDefenderStats _stats;

    public void Init(TowerDefenderConfig config)
    {
        _projectilePrefab = config.ProjectilePrefab;
        _modifierConfig = config.Modifier;
        _stats = config.Stats;
        _skeletonMecanim.skeletonDataAsset = config.Skeleton;
        _skeletonMecanim.Initialize(true);
        _animator.runtimeAnimatorController = config.AnimatorController;
    }
}