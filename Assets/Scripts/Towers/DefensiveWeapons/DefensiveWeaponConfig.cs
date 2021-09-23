using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

public class DefensiveWeaponConfig : ScriptableObject
{
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public DefensiveWeaponStats Stats;
}