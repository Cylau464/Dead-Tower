using System;
using UnityEngine;

public class Game : MonoBehaviour
{
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
    }

	public void LevelEnd(bool playerWin)
	{
		OnLevelEnd?.Invoke(playerWin);
	}
}
