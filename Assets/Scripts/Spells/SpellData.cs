using UnityEngine;

public class SpellData
{
    public string name;
    public string description;
    public int icon;
    public string N;
    public DamageData damage;
    public float base_damage;

    public string secondary_damage;
    public float base_secondary_damage;

    public string mana_cost;
    public int base_mana_cost;

    public string cooldown;
    public float base_cooldown;

    public ProjectileData projectile;
    public ProjectileData secondary_projectile;
    public int N_value;
}

public class DamageData
{
    public string amount;
    public string type;
}

public class ProjectileData
{
    public string trajectory;
    public string speed;
    public string sprite;
    public string lifetime;
    public float base_speed;
    public float base_lifetime;
}