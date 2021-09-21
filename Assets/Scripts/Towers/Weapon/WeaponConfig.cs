using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Weapon", menuName = "TowerWeapon")]
public class WeaponConfig : ScriptableObject
{
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public GameObject ProjectilePrefab;
}