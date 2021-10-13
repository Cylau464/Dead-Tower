using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool _isLevelEnd { get; private set; }

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

	public void LevelEnd(bool playerWin, bool forceEnd = false)
	{
        if (_isLevelEnd == true && forceEnd == false) return;

		OnLevelEnd?.Invoke(playerWin);
        _isLevelEnd = true;
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadComplete -= ResetLevelState;
    }
}
