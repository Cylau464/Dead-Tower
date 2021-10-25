using UnityEngine;
using UnityEngine.UI;

public class SkullRandomizer : MonoBehaviour
{
    [SerializeField] private float _chance = .1f;
    [SerializeField] private SpriteRenderer _bgRenderer;
    [SerializeField] private SpriteRenderer[] _groundRenderers;
    [Space]
    [SerializeField] private Sprite _bgSprite;
    [SerializeField] private Sprite _groundSprite;

    public bool SkullEnabled;

    public static SkullRandomizer Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        if(EnemySpawner.LevelConfig.Difficulty.BossLevel == false)
        {
            SkullEnabled = Random.value <= _chance;

            if(SkullEnabled == true)
            {
                _bgRenderer.sprite = _bgSprite;

                foreach (SpriteRenderer renderer in _groundRenderers)
                {
                    renderer.sprite = _groundSprite;
                    renderer.size = new Vector2(renderer.size.x, _groundSprite.bounds.size.y);
                }
            } 
        }
    }
}