using UnityEngine;

public class Modifier : MonoBehaviour
{
    protected float _duration = 0f;
    protected float _value = 0f;

    public void Init(float duration, float value)
    {
        _duration = duration;
        _value = value;
    }
}