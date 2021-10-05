using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private bool _isLevelEnd;

    public static Game Instance;

    //public event Action<int> OnLevelStart;
    public event Action<bool> OnLevelEnd;

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
        _isLevelEnd = false;
    }

	public void LevelEnd(bool playerWin)
	{
        if (_isLevelEnd == true) return;

		OnLevelEnd?.Invoke(playerWin);
        _isLevelEnd = true;
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadComplete -= ResetLevelState;
    }
}
