using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	public StoredValue<int> Coins;
	public StoredValue<int> Diamonds;
	public StoredValue<Resource[]> Resources;
	public StoredValue<int[]> ProjectilesCount;
	public StoredValue<TowerData[]> Towers;
	public StoredValue<DefenderData[]> Defenders;

	public GameData()
	{
		if (AssetsHolder.Instance == null) return;

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
			towers[config.Index] = new TowerData(config.Stats, config.PurchaseStats);
		}

		Towers = new StoredValue<TowerData[]>(towers);
	}

	private void GetDefenders()
    {
		DefenderData[] defenders = new DefenderData[AssetsHolder.Instance.DefenderConfigs.Length];

		for (int i = 0; i < defenders.Length; i++)
		{
			TowerDefenderConfig config = AssetsHolder.Instance.DefenderConfigs[i];
			defenders[config.Index] = new DefenderData(config.Stats, config.PurchaseStats);
		}

		Defenders = new StoredValue<DefenderData[]>(defenders);
	}

	private void GetProjectiles()
    {
		int[] projectiles = new int[AssetsHolder.Instance.ProjectileConfigs.Length];

		for(int i = 0; i < projectiles.Length; i++)
			projectiles[AssetsHolder.Instance.ProjectileConfigs[i].Index] = 50;

		ProjectilesCount = new StoredValue<int[]>(projectiles);
	}
}