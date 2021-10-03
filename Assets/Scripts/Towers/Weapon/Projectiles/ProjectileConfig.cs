using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Config")]
public class ProjectileConfig : ScriptableObject
{
    public int Index;
    public string Title;
    public Projectile Prefab;
    public Sprite Icon;
    public ProjectileStats Stats;
    public ProjectileRecipe Recipe;
}