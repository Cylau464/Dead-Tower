using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float _minSpeed = -5f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _minAcceleration = .001f;
    [SerializeField] private float _maxAcceleration = .009f;
    [SerializeField] private float _minChangeDelay = 30f;
    [SerializeField] private float _maxChangeDelay = 60f;

    public static float strength;

    private float _startStrength;
    private float _targetStrength;
    private float _speed;
    private float _lerpTime;

    private void Start()
    {
        _speed = Random.Range(_minAcceleration, _maxAcceleration);
        _targetStrength = Random.Range(_minSpeed, _maxSpeed);

        StartCoroutine(WindChoose());
    }

    private void Update()
    {
        strength = Mathf.Lerp(_startStrength, _targetStrength, _lerpTime);
        _lerpTime += Time.deltaTime * _speed;
    }

    private IEnumerator WindChoose()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minChangeDelay, _maxChangeDelay));
            _speed = Random.Range(_minAcceleration, _maxAcceleration);
            _startStrength = strength;
            _targetStrength = Random.Range(_minSpeed, _maxSpeed);
            _lerpTime = 0f;
        }
    }
}