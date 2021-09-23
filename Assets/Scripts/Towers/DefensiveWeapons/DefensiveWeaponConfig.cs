using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Defensive Weapon", menuName = "Tower/Defensive Weapon")]
public class DefensiveWeaponConfig : ScriptableObject
{
    public DefensiveWeapon Prefab;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public DefensiveWeaponStats Stats;
}