using UnityEngine;
using Spine.Unity;

public abstract class UnitBasicConfig : ScriptableObject
{
    public int Index;
    public string Title;
    public string Description;
    public Sprite MenuIcon;
    public SkeletonDataAsset Skeleton;
    public RuntimeAnimatorController AnimatorController;
    public PurchaseStats PurchaseStats;
}