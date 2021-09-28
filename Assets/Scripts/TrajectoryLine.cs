using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Range(2, 30)] [SerializeField] private int _resolution = 10;
    [SerializeField] private LineRenderer _line = null;
    [SerializeField] private float _yLimit;
    [SerializeField] private LayerMask _targetLayer = 0;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _textureOffsetSpeed = 2f;
    private float _gravity;
    private Vector2 _velocity, _shootPoint;
    private Material _lineMaterial;

    private void Start()
    {
        _lineMaterial = _line.material;
        _gravity = Mathf.Abs(Physics2D.gravity.y);
        gameObject.SetActive(false);
    }

    public void SetYLimit(Vector3 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 100f, _groundLayer);

        if (hit == true)
            _yLimit = hit.point.y;
    }

    private void OnDisable()
    {
        _lineMaterial.SetTextureOffset("_MainTex", Vector2.zero);
    }

    public void UpdateLine(Vector2 velocity, Vector2 shootPoint, bool textureOffset = false)
    {
        _velocity = velocity;
        _shootPoint = shootPoint;
        _line.positionCount = _resolution + 1;
        _line.SetPositions(CalculateLineArray());

        if(textureOffset == true)
        {
            Vector2 offset = Vector2.left * _textureOffsetSpeed * Time.deltaTime;
            _lineMaterial.SetTextureOffset("_MainTex", _lineMaterial.GetTextureOffset("_MainTex") + offset);
        }
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[_resolution + 1];
        float lowestTimeValue = MaxTimeX() / _resolution;

        for (int i = 0; i < lineArray.Length; i++)
        {
            float t = i * lowestTimeValue;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = _velocity.x * t;
        float y = (_velocity.y * t) - (_gravity * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + _shootPoint.x, y + _shootPoint.y);
    }

    private float MaxTimeY()
    {
        float v = _velocity.y;
        float vSq = Mathf.Pow(v, 2);
        float t = (v + Mathf.Sqrt(vSq + 2 * _gravity * (_shootPoint.y - Mathf.Min(_yLimit, _shootPoint.y)))) / _gravity;

        return t;
    }

    private float MaxTimeX()
    {
        float x = _velocity.x;
        float t = (HitPosition().x - _shootPoint.x) / x;

        return t;
    }

    private Vector2 HitPosition()
    {
        float lowestTimeValue = MaxTimeY() / _resolution;

        for (int i = 0; i < _resolution + 1; i++)
        {
            float t = lowestTimeValue * i;
            float nextT = lowestTimeValue * (i + 1);
            RaycastHit2D hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(nextT), _targetLayer);

            if (hit)
                return hit.point;

        }

        return CalculateLinePoint(MaxTimeY());
    }
}
