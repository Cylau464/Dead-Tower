using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyConfig : ScriptableObject
{
    public string Name;
    public KillTargets Type;
    public Enemy Prefab;
    public SkeletonDataAsset Skeleton;
    public RuntimeAnimatorController AnimatorController;
    public EnemyStats Stats;
    public Rewards Rewards;
}
