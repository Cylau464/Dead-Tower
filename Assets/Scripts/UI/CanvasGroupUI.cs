using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeTime = 0.2f;

	private bool _isInitialized;

    protected void Awake()
    {
		if (_isInitialized == false)
			Init();
    }

    protected virtual void Init()
    {
		_isInitialized = true;
    }

    public virtual void Show()
	{
		StopAllCoroutines();
		gameObject.SetActive(true);

		if (_isInitialized == false)
			Init();

		this.LerpCoroutine(
			time: fadeTime,
			from: canvasGroup.alpha,
			to: 1,
			action: a => canvasGroup.alpha = a
		);
	}

	public virtual void Hide()
	{
		StopAllCoroutines();
		this.LerpCoroutine(
			time: fadeTime,
			from: canvasGroup.alpha,
			to: 0,
			action: a => canvasGroup.alpha = a,
			onEnd: () => gameObject.SetActive(false)
		);
	}

	//protected IEnumerator LerpCoroutine(float time, float from, float to, Action<float> action, Action onEnd = null)
 //   {
	//	float t = 0f;

	//	while(t < 1f)
 //       {
	//		t += Time.unscaledDeltaTime / time;
	//		action?.Invoke(Mathf.Lerp(from, to, t));

	//		yield return null;
 //       }

	//	onEnd?.Invoke();
 //   }
}