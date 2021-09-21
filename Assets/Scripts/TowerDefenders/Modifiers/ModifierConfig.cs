using UnityEngine;

public abstract class ModifierConfig : ScriptableObject
{
    public Modifier Class;
    public float Duration;
    public float Value;
}