using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Weapon", menuName = "Tower/Weapon")]
public class WeaponConfig : ScriptableObject
{
    public SkeletonDataAsset Skeleton;
    public RuntimeAnimatorController AnimatorController;
    public ProjectileConfig ProjectileConfig;
    public WeaponStats Stats;
    [Header("Audio Clips")]
    public AudioClip ChargeClip;
    public AudioClip ShotClip;
}