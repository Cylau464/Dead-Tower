using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyConfig : ScriptableObject
{
    public string Name;
    public Enemy Prefab;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public EnemyStats Stats;
    public Rewards Rewards;
}
