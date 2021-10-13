using UnityEngine;

public class StagesInitializer : MonoBehaviour
{
    [SerializeField] private StageConfig[] _stageConfigs;
    [SerializeField] private StageLoader _stageLoader;
    [Space]
    [SerializeField] private RectTransform _contentHolder;
    [SerializeField] private RectTransform _layoutGroup;

    private void Start()
    {
        float width = 0f;

        foreach (StageConfig config in _stageConfigs)
        {
            width += config.Sprite.texture.width;
            StageLoader loader = Instantiate(_stageLoader);
            loader.transform.SetParent(_layoutGroup, false);
            loader.Init(config);
        }

        _contentHolder.sizeDelta = new Vector2(width, _contentHolder.sizeDelta.y);
        _contentHolder.position = new Vector2(0f, _contentHolder.position.y);
        _layoutGroup.position = new Vector2(0f, _contentHolder.position.y);
        _layoutGroup.sizeDelta = new Vector2(width, _contentHolder.sizeDelta.y);
    }
}
