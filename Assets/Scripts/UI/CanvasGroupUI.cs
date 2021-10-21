using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float _fadeTime = 0.2f;
	[SerializeField] private bool _lerpOnPause = false;
	[Space]
	[SerializeField] protected AudioClip _buttonClip;

	private bool _isInitialized;

    protected void Awake()
    {
		if (_isInitialized == false)
			Init();
    }

    protected virtual void Init()
    {
		canvasGroup.alpha = 0f;
		_isInitialized = true;
    }

    public virtual void Show()
	{
		StopAllCoroutines();
		gameObject.SetActive(true);

		if (_isInitialized == false)
			Init();

		if((Time.timeScale <= 0f && _lerpOnPause == true) || Time.timeScale > 0f)
        {
			this.LerpCoroutine(
				time: _fadeTime,
				from: canvasGroup.alpha,
				to: 1f,
				action: a => canvasGroup.alpha = a,
				settings: new CoroutineTemplate.Settings(
					lerpOnPause: _lerpOnPause
					)
			);
        }
		else
        {
			canvasGroup.alpha = 1f;
        }
	}

	public virtual void Hide()
	{
		StopAllCoroutines();

		if (gameObject.activeInHierarchy == false) return;

		if ((Time.timeScale <= 0f && _lerpOnPause == true) || Time.timeScale > 0f)
		{
			this.LerpCoroutine(
				time: _fadeTime,
				from: canvasGroup.alpha,
				to: 0f,
				action: a => canvasGroup.alpha = a,
				onEnd: () => gameObject.SetActive(false)
			);
		}
		else
        {
			canvasGroup.alpha = 0f;
			gameObject.SetActive(false);
        }
	}
}