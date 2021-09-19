using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static int _menuSceneIndex = 0;
    private static int _levelSceneIndex = 1;
    public static Action OnSceneLoad;

    public static Action LoadLevel()
    {
        Load(_levelSceneIndex);
        return OnSceneLoad;
    }

    public static Action LoadMenu()
    {
        Load(_menuSceneIndex);
        return OnSceneLoad;
    }

    private static void Load(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        //operation.allowSceneActivation = false;
        operation.completed += AsyncLoadComplete;
    }

    private static void AsyncLoadComplete(AsyncOperation operation)
    {
        //operation.allowSceneActivation = true;
        operation.completed -= AsyncLoadComplete;
        OnSceneLoad?.Invoke();
    }
}
