using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeTime = 0.2f;
	[SerializeField] private float _showDelay = 0f;

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
}