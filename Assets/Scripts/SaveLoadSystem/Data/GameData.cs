using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	public StoredValue<List<LevelConfig[]>> Levels;
	public StoredValue<LevelConfig> LastLevel;
	public StoredValue<int> Coins;
	public StoredValue<int> Diamonds;
	public StoredValue<Resource[]> Resources;
	public StoredValue<int[]> ProjectilesCount;
	public StoredValue<TowerData[]> Towers;
	public StoredValue<DefenderData[]> Defenders;
	public StoredValue<TowerData> SelectedTower;
	public StoredValue<DefenderData> SelectedDefender;

	public GameData()
	{
		if (AssetsHolder.Instance == null) return;

		Levels = new StoredValue<List<LevelConfig[]>>();
		LastLevel = new StoredValue<LevelConfig>();
		Coins = new StoredValue<int>(0);
		Diamonds = new StoredValue<int>(0);
		GetResources();
		GetTowers();
		GetDefenders();
		GetProjectiles();
	}

	private void GetResources()
    {
		List<Resource> resources = new List<Resource>(AssetsHolder.Instance.ResourceConfigs.Length);

		foreach (ResourceConfig config in AssetsHolder.Instance.ResourceConfigs)
			resources.Add(new Resource(config.Type, 10));

		Resources = new StoredValue<Resource[]>(resources.ToArray());
	}

	private void GetTowers()
    {
		TowerData[] towers = new TowerData[AssetsHolder.Instance.TowerConfigs.Length];

		for (int i = 0; i < towers.Length; i++)
		{
			TowerConfig config = AssetsHolder.Instance.TowerConfigs[i];
			towers[config.Index] = new TowerData(config.Index, config.Stats, config.PurchaseStats.IsPurchased);
		}

		SelectedTower = new StoredValue<TowerData>(towers[0]);
		Towers = new StoredValue<TowerData[]>(towers);
	}

	private void GetDefenders()
    {
		DefenderData[] defenders = new DefenderData[AssetsHolder.Instance.DefenderConfigs.Length];

		for (int i = 0; i < defenders.Length; i++)
		{
			TowerDefenderConfig config = AssetsHolder.Instance.DefenderConfigs[i];
			defenders[config.Index] = new DefenderData(config.Index, config.Stats, config.PurchaseStats.IsPurchased);
		}

		SelectedDefender = new StoredValue<DefenderData>(defenders[0]);
		Defenders = new StoredValue<DefenderData[]>(defenders);
	}

	private void GetProjectiles()
    {
		int[] projectiles = new int[AssetsHolder.Instance.ProjectileConfigs.Length];

		for(int i = 0; i < projectiles.Length; i++)
			projectiles[AssetsHolder.Instance.ProjectileConfigs[i].Index] = 50;

		ProjectilesCount = new StoredValue<int[]>(projectiles);
	}

	public LevelConfig GetNextLevel()
    {
		if (LastLevel.Value == null)
        {
			return Levels.Value[0][0];
        }
		else
        {
			if (Levels.Value[LastLevel.Value.StageIndex].Length > LastLevel.Value.Number + 1)
				return Levels.Value[LastLevel.Value.StageIndex][LastLevel.Value.Number + 1];
			else if (Levels.Value.Count > LastLevel.Value.StageIndex + 1)
				return Levels.Value[LastLevel.Value.StageIndex + 1][0];

			UnityEngine.Debug.LogWarning("This was the last level");
			return null;
		}
    }
}