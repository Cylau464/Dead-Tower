using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _defeatClip;

    public bool IsLevelEnd { get; private set; }

    public static Game Instance;

    //public event Action<int> OnLevelStart;
    public event Action<bool> OnLevelEnd;
    public Action OnLevelContinued;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        SceneLoader.OnSceneLoadComplete += ResetLevelState;
    }

    private void ResetLevelState()
    {
        IsLevelEnd = false;
    }

	public void LevelEnd(bool playerWin, bool forceEnd = false)
	{
        if (IsLevelEnd == true && forceEnd == false) return;

        if (playerWin == true)
            AudioController.PlayClipAtPosition(_victoryClip, transform.position);
        else
            AudioController.PlayClipAtPosition(_defeatClip, transform.position);

		OnLevelEnd?.Invoke(playerWin);
        IsLevelEnd = true;
    }

    public void ContinueLevel()
    {
        OnLevelContinued?.Invoke();
        IsLevelEnd = false;
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadComplete -= ResetLevelState;
    }
}
