using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeTime = 0.2f;

	public virtual void Show()
	{
		gameObject.SetActive(true);

		StopAllCoroutines();
		StartCoroutine(LerpCoroutine(
			time: fadeTime,
			from: canvasGroup.alpha,
			to: 1,
			action: a => canvasGroup.alpha = a
		));
	}

	public virtual void Hide()
	{
		StopAllCoroutines();
		StartCoroutine(LerpCoroutine(
			time: fadeTime,
			from: canvasGroup.alpha,
			to: 0,
			action: a => canvasGroup.alpha = a,
			onEnd: () => gameObject.SetActive(false)
		));
	}

	protected IEnumerator LerpCoroutine(float time, float from, float to, Action<float> action, Action onEnd = null)
    {
		float t = 0f;

		while(t < 1f)
        {
			t += Time.deltaTime / time;
			action?.Invoke(Mathf.Lerp(from, to, t));

			yield return null;
        }

		onEnd?.Invoke();
    }
}