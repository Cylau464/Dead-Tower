using UnityEngine;
using UnityEngine.UI;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private StageFactory _factory;
    //[SerializeField] private LineRenderer _line;

    //private void Start()
    //{
    //    Level[] levels = _factory.GetStageLevels(_config);

    //    for(int i = 0; i < levels.Length; i++)
    //    {
    //        levels[i].transform.position = _line.GetPosition(i);
    //        levels[i].transform.SetParent(transform, false);
    //    }
    //}

    public void Init(StageConfig config)
    {
        Level[] levels = _factory.GetStageLevels(config);
        _background.sprite = config.Sprite;

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].transform.position = config.SpawnCurve.GetPosition(i);
            levels[i].transform.SetParent(transform, false);
        }

        RectTransform rect = (transform as RectTransform);
        rect.sizeDelta = new Vector2(config.Sprite.texture.width, rect.sizeDelta.y);
    }
}
