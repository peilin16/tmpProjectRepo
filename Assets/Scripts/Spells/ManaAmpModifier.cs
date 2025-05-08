using UnityEngine;

public class ManaAmpModifier : ModifierSpell
{
    private float manaMultiplier;

    public ManaAmpModifier(float multiplier)
    {
        this.manaMultiplier = multiplier;
    }

    public override Spell Application(Spell spell)
    {
        spell.final_mana_cost = Mathf.RoundToInt(spell.final_mana_cost * manaMultiplier);
        return spell;
    }
}
