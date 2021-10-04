using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static int _menuSceneIndex = 0;
    private static int _levelSceneIndex = 1;
    public static Action OnSceneLoadStart;
    public static Action OnSceneLoadComplete;

    public static void LoadLevel()
    {
        Load(_levelSceneIndex);
    }

    public static void LoadMenu()
    {
        Load(_menuSceneIndex);
        MenuSwitcher.OpenMapAfterLoad = true;
    }

    private static void Load(int index)
    {
        OnSceneLoadStart?.Invoke();
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        //operation.allowSceneActivation = false;
        operation.completed += AsyncLoadComplete;
    }

    private static void AsyncLoadComplete(AsyncOperation operation)
    {
        //operation.allowSceneActivation = true;
        operation.completed -= AsyncLoadComplete;
        OnSceneLoadComplete?.Invoke();
    }
}
