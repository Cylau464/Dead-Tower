using System;

[Serializable]
public class SettingsData
{
	public StoredValue<bool> SoundEnabled;
	public StoredValue<bool> MusicEnabled;
	public StoredValue<bool> AdsEnabled;
	public StoredValue<bool> VibrationEnabled;
	public StoredValue<bool> TutorialPassed;

	public SettingsData()
	{
		SoundEnabled = new StoredValue<bool>(true);
		MusicEnabled = new StoredValue<bool>(true);
		VibrationEnabled = new StoredValue<bool>(true);
		AdsEnabled = new StoredValue<bool>(true);
		TutorialPassed = new StoredValue<bool>(false);
	}
}