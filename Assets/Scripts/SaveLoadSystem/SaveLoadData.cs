using System;

[Serializable]
public class SaveLoadData
{
	public GameData Game;
	public SettingsData Settings;
	public QuestsData Quests;

	public SaveLoadData()
	{
		Game = new GameData();
		Settings = new SettingsData();
		Quests = new QuestsData();
	}
}