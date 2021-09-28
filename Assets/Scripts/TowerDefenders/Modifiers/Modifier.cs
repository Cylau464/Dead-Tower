using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
    protected float _duration = 0f;
    protected float _value = 0f;

    public void Init(float duration, float value)
    {
        _duration = duration;
        _value = value;
    }

    private void LateUpdate()
    {
        Timer();
    }

    private void OnDestroy()
    {
        CancelEffect();
    }

    protected abstract void Timer();
    protected abstract void ApplyEffect();
    protected abstract void CancelEffect();
}