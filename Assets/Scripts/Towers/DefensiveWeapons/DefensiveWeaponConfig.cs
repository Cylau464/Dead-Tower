using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Defensive Weapon", menuName = "Tower/Defensive Weapon")]
public class DefensiveWeaponConfig : ScriptableObject
{
    public DefensiveWeapon Prefab;
    public SkeletonDataAsset Skeleton;
    public RuntimeAnimatorController AnimatorController;
    public DefensiveWeaponStats Stats;
    [Space]
    public AudioClip DestroyClip;
}