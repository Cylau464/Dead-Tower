using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeOutTime = .2f;
    [SerializeField] private Slider _slider;

    [Header("Damage Bar")]
    [SerializeField] private Image _damageBar;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _decreaseSpeed = 2f;
    [SerializeField] private float _decreaseReducer = .1f;

    private IDamageTaker _damageTaker;
    private bool _defenderIsDead;

    private void Start()
    {
        if (transform.parent.TryGetComponent(out IDamageTaker damageTaker))
            _damageTaker = damageTaker;
        else
            Debug.LogError("Cant find IDamageTaker on parent object");

        _slider.value = 1f;
        _damageBar.fillAmount = 1f;
        _damageTaker.HealthChanged += UpdateBar;
    }

    public void UpdateBar(object sender, float health)
    {
        StopAllCoroutines();

        if (health <= 0f)
            StartCoroutine(FadeOut());
        else if (_defenderIsDead == true)
            StartCoroutine(FadeIn());

        _slider.value = health;
        StartCoroutine(UpdateDamageBar(health));
    }

    private IEnumerator UpdateDamageBar(float health)
    {
        yield return new WaitForSeconds(_duration);

        float t = 0f;
        float startValue = _damageBar.fillAmount;
        float speed = _decreaseSpeed;

        while(t < 1f)
        {
            t += Time.deltaTime * speed;
            speed = Mathf.Max(speed - _decreaseReducer * Time.deltaTime, .05f);
            _damageBar.fillAmount = Mathf.Lerp(startValue, health, t);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeIn()
    {
        _defenderIsDead = false;
        float t = 0f;
        float startValue = _canvasGroup.alpha;
        float targetValue = 1f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadeOutTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut()
    {
        _defenderIsDead = true;
        float t = 0f;
        float startValue = _canvasGroup.alpha;
        float targetValue = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime / _fadeOutTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        if(_damageTaker != null)
            _damageTaker.HealthChanged -= UpdateBar;
    }
}