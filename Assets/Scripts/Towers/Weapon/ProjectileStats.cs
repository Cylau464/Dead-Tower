public struct ProjectileStats
{
    public int Damage;
    public ModifierConfig ModifierConfig;

    public ProjectileStats(int damage, ModifierConfig modifier)
    {
        Damage = damage;
        ModifierConfig = modifier;
    }
}