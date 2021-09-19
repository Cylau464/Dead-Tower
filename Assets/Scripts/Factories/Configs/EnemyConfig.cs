using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyConfig : ScriptableObject
{
    public string Name;
    public Enemy Prefab;
    public SkeletonDataAsset SkeletonData;
    public EnemyStats Stats;
    public Rewards Rewards;
}
