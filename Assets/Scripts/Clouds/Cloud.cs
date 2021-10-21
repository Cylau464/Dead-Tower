using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite[] _sprites;
    [Space]
    [SerializeField] private float _fadeInTime = 1f;
    [SerializeField] private float _minScale = .8f;
    [SerializeField] private float _maxScale = 1.2f;

    private CloudSpawner _spawner;

    public void Init(CloudSpawner spawner)
    {
        _spawner = spawner;
    }

    private void Awake()
    {
        Color color = _renderer.color;
        color.a = 0f;
        _renderer.color = color;
    }

    private void OnEnable()
    {
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        transform.localScale = Vector3.one * Mathf.Lerp(_minScale, _maxScale, Random.value);

        this.LerpCoroutine(
            time: _fadeInTime,
            from: _renderer.color.a,
            to: 1f,
            action: a =>
            {
                Color color = _renderer.color;
                color.a = a;
                _renderer.color = color;
            }
        );
    }

    private void FixedUpdate()
    {
        Vector2 force = Vector2.right * Wind.strength / 5f * transform.localScale.x;
        _rigidBody.AddForce(force, ForceMode2D.Force);
    }

    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy == false) return;

        StopAllCoroutines();
        this.LerpCoroutine(
            time: _fadeInTime,
            from: _renderer.color.a,
            to: 0f,
            action: a =>
            { 
                Color color = _renderer.color;
                color.a = a;
                _renderer.color = color;
            },
            onEnd: ReturnToPool
        );        
    }

    private void ReturnToPool()
    {
        _spawner.ReturnToPool(this);
        gameObject.SetActive(false);
    }
}
