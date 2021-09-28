using UnityEngine;

[CreateAssetMenu(fileName = "Slowin Modifier", menuName = "Modifiers/Slowing")]
public class SlowingModifierConfig : ModifierConfig
{
    public override void ApplyToTarget(GameObject target)
    {
        SlowingModifier mod = target.AddComponent<SlowingModifier>();
        mod.Init(Duration, Value);
    }

    public override bool HasModifier(GameObject target)
    {
        return target.TryGetComponent(out SlowingModifier modifier);
    }
}