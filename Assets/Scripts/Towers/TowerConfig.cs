using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower/Tower")]
public class TowerConfig : ScriptableObject
{
    public Sprite MenuSprite;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public DefensiveWeaponConfig DefensiveWeaponConfig;
    public WeaponConfig WeaponConfig;
    public TowerStats Stats;
}