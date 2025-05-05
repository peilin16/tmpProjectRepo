using UnityEngine;

public class SpeedAmpModifier : ModifierSpell
{
    private float speedMultiplier;
    private float finalSpeed;
    public SpeedAmpModifier(float mult)
    {
        this.speedMultiplier = mult;
    }
    public override Spell Application(Spell spell)
    {
        spell.final_speed *= speedMultiplier;
        if (spell.data.secondary_projectile != null)
            spell.data.secondary_projectile.base_speed *= speedMultiplier;
        return spell;
    }

    
}