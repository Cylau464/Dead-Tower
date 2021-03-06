using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

// Короткое обращение к SaveLoadData
public static class SLS
{
	public static SaveLoadData Data => SaveLoadSystem.Instance.Data;

	public static void Save()
	{
		SaveLoadSystem.Instance.SaveData();
	}
}

public class SaveLoadSystem : MonoBehaviour
{
	[SerializeField] private float saveDelay = 1;
	[SerializeField] private bool _resetSaves = false;
	private float delay;
	private bool needSave;

	public static SaveLoadSystem Instance { get; private set; }

	public SaveLoadData Data { get; private set; }

	private const string DataKey = "Data";

	private void Awake()
	{
		if (Instance != null && Instance != this)
        {
			Destroy(this);
			return;
        }

		Instance = this;

		if(_resetSaves == true)
			PlayerPrefs.SetString(DataKey, "");

		var ppData = PlayerPrefs.GetString(DataKey, "");

		if (string.IsNullOrEmpty(ppData))
        {
			Data = new SaveLoadData();
        }
		else
        {
			Data = JsonConvert.DeserializeObject<SaveLoadData>(ppData);
			Data.Game.SelectedDefender.Value = Data.Game.Defenders.Value[Data.Game.SelectedDefender.Value.Index];
			Data.Game.SelectedTower.Value = Data.Game.Towers.Value[Data.Game.SelectedTower.Value.Index];
        }

		DontDestroyOnLoad(this);
	}

	public void Save()
	{
		if (delay > 0)
		{
			needSave = true;
			return;
		}

		SaveData();
	}

	public void SaveData()
	{
		if (Data == null)
			return;

		var json = JsonConvert.SerializeObject(Data);
		PlayerPrefs.SetString(DataKey, json);

		needSave = false;
		StartCoroutine(SaveTimer());
	}

	private IEnumerator SaveTimer()
	{
		delay = saveDelay;

		for (; delay > 0; delay -= Time.deltaTime)
			yield return null;

		delay = 0;

		if (needSave)
			SaveData();
	}
}