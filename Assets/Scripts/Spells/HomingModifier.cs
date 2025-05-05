using UnityEngine;


public class HomingModifier : ModifierSpell
{
    private float damageMultiplier;
    private int manaAdder;

    public HomingModifier(float damageMult, int manaAdd)
    {
        this.damageMultiplier = damageMult;
        this.manaAdder = manaAdd;
    }
    public override Spell Application(Spell spell)
    {
        spell.final_damage = Mathf.RoundToInt(spell.final_damage * damageMultiplier);
        spell.final_mana_cost += manaAdder;

        //if (spell.data.projectile != null)
        spell.final_trajectory = "homing";

        return spell;
    }

}