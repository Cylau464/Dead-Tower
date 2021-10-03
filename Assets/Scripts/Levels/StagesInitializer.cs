using UnityEngine;
using UnityEngine.UI;

public class StagesInitializer : MonoBehaviour
{
    [SerializeField] private StageConfig[] _stageConfigs;
    [SerializeField] private StageLoader _stageLoader;
    [Space]
    [SerializeField] private RectTransform _contentHolder;

    private void Start()
    {
        float width = 0f;

        foreach (StageConfig config in _stageConfigs)
        {
            width += config.Sprite.texture.width;
            StageLoader loader = Instantiate(_stageLoader);
            loader.transform.SetParent(_contentHolder, false);
            loader.Init(config);
        }

        _contentHolder.sizeDelta = new Vector2(width, _contentHolder.sizeDelta.y);
        _contentHolder.position = new Vector2(0f, _contentHolder.position.y);
    }
}
