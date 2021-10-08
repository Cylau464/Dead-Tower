using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;

public class DissolveImage : MonoBehaviour
{
    [SerializeField] private Material _dissolveMaterial;
    [SerializeField] private Image _image;
    [Space]
    [SerializeField] private float _dissolveTime = .5f;
    //[SerializeField] private float _colorIntensity = 6f;

    private Material _material;

    public void SetSprite(Sprite sprite)
    {
        bool veryOld = (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2);

        if(veryOld == false)
        {
            if (_dissolveMaterial != null && _image.material != _dissolveMaterial)
                _material = _image.material = new Material(_dissolveMaterial);
        }
        else
        {
            _image.material = null;
        }

        StartCoroutine(Dissolve(sprite));
    }

    private IEnumerator Dissolve(Sprite sprite)
    {
        if (_material == null)
            _material = _image.material = new Material(_image.material);

        float t = 0f;
        //_material.SetColor("_DissolveColor", AverageColorFromTexture(_image.sprite.texture, _colorIntensity));
        bool spriteChanged = false;

        while(t < 1f)
        {
            t += Time.deltaTime / _dissolveTime;

            if(t >= .5f && spriteChanged == false)
            {
                _image.sprite = sprite;
                //_material.SetColor("_DissolveColor", AverageColorFromTexture(_image.sprite.texture, _colorIntensity));
                spriteChanged = true;
            }

            _material.SetFloat("_DissolveAmount", 1f - Mathf.PingPong(t * 2f, 1f));

            yield return null;
        }

        _material.SetFloat("_DissolveAmount", 1f);
    }

    //private Color AverageColorFromTexture(Texture2D tex, float intensity)
    //{

    //    Color[] texColors = tex.GetPixels();

    //    int total = texColors.Length;

    //    float r = 0;
    //    float g = 0;
    //    float b = 0;

    //    for (int i = 0; i < total; i++)
    //    {

    //        r += texColors[i].r;

    //        g += texColors[i].g;

    //        b += texColors[i].b;

    //    }

    //    float factor = Mathf.Pow(2f, intensity);
    //    return new Color(r / total * factor, g / total * factor, b / total * factor, 255f);

    //}
}