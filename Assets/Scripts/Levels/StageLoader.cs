using UnityEngine;
using UnityEngine.UI;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private StageFactory _factory;

    public void Init(StageConfig config)
    {
        Level[] levels = _factory.GetStageLevels(config);
        _background.sprite = config.Sprite;
        float refAspectRation = 1920f / 1080f;
        float curAspectRation = (float)Screen.width / Screen.height;
        float aspectRatioOffset = (refAspectRation - curAspectRation) / refAspectRation;
        LineRenderer curve = Instantiate(config.SpawnCurve);
        curve.transform.SetParent(transform, false);
        curve.transform.localScale += Vector3.up * aspectRatioOffset;

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].transform.position =
                transform.InverseTransformPoint(
                curve.transform.TransformPoint(
                curve.GetPosition(i)));
            levels[i].transform.SetParent(transform, false);
        }

        Destroy(curve.gameObject);
        RectTransform rect = (transform as RectTransform);
        rect.sizeDelta = new Vector2(config.Sprite.texture.width, rect.sizeDelta.y);
    }
}
