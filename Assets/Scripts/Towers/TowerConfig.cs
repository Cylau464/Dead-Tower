using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower")]
public class TowerConfig : ScriptableObject
{
    public Sprite MenuSprite;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public GameObject AbilityPrefab;
    public WeaponConfig WeaponConfig;
    public TowerStats Stats;
}