using UnityEngine;

public class ScaleMover : MonoBehaviour
{
    private float _startX;

    private void Start()
    {
        _startX = transform.position.x;
        float refWidth = 19.2f;
        float height = Camera.main.orthographicSize * 2f;
        float width = height * ((float)Screen.width / Screen.height);
        float offset = (refWidth / 2f - Mathf.Abs(_startX)) / refWidth;
        float newPos = offset * width;
        newPos = width / 2f - newPos;

        transform.position = new Vector3(Mathf.Sign(_startX) * newPos, transform.position.y, transform.position.z);
    }
}
