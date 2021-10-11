using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteScaler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    private void Start()
    {
        Vector2 spriteSize = _renderer.size;
        float height = Camera.main.orthographicSize * 2f;
        float width = height * ((float)Screen.width / Screen.height);
        spriteSize.x *= width / spriteSize.x;
        _renderer.size = new Vector2(width, spriteSize.y);
    }
}
