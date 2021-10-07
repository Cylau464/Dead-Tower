using UnityEngine;

public enum ItemCategory { Tower, Defender, Resources, Currency }
public enum CurrencyTypes { Coins, Diamonds }

public abstract class ShopItemConfig : ScriptableObject
{
    public ShopItem Prefab;
    public Sprite Icon;
    public PurchaseStats Stats;
    public ItemCategory Category;
}

public abstract class MPIConfig : ShopItemConfig
{
    public int Count;
}