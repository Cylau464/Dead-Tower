using UnityEngine;

public abstract class ModifierConfig : ScriptableObject
{
    public float Duration;
    public float Value;

    public abstract void ApplyToTarget(GameObject target);
    public abstract bool HasModifier(GameObject target);
}