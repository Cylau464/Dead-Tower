using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Weapon", menuName = "Tower/Weapon")]
public class WeaponConfig : ScriptableObject
{
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public GameObject ProjectilePrefab;
}