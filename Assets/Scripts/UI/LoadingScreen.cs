using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingScreen : CanvasGroupUI
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _loadingText = "Loading";
    [SerializeField] private float _textUpdateDelay = 1f;
    private Material _textMaterial;

    public static LoadingScreen Instance;

    protected override void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneLoader.OnSceneLoadStart += Show;
        SceneLoader.OnSceneLoadComplete += Hide;
        gameObject.SetActive(false);
        _textMaterial = _text.fontMaterial;

        base.Init();
    }

    public override void Show()
    {
        base.Show();
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        int points = 0;
        float glowMin = .3f;
        float glowMax = .75f;
        float t = 0f;
        float delay = 0f;

        while (true)
        {
            t += Time.deltaTime;
            delay -= Time.deltaTime;
            float glowOffset = Mathf.PingPong(t, glowMax - glowMin);
            _textMaterial.SetFloat("_GlowPower", Mathf.Max(glowMin, glowMin + glowOffset));

            if(delay <= 0f)
            {
                _text.text = _loadingText + new string('.', points);

                if (++points > 3)
                    points = 0;

                delay = _textUpdateDelay;
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadStart -= Show;
        SceneLoader.OnSceneLoadComplete -= Hide;
    }
}