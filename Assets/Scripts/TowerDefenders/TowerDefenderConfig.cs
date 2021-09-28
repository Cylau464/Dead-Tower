using UnityEngine;
using Spine.Unity;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Tower Defender", menuName = "Tower/Defender")]
public class TowerDefenderConfig : ScriptableObject
{
    public Sprite MenuSprite;
    public SkeletonDataAsset Skeleton;
    public AnimatorController AnimatorController;
    public Projectile ProjectilePrefab;
    public ModifierConfig Modifier;
    public TowerDefenderStats Stats;
}