using UnityEngine;

public class DamageAmpModifier : ModifierSpell
{
    private float damageMultiplier;
    private float manaMultiplier;

    public DamageAmpModifier(float damageMult, float manaMult)
    {
        this.one_time = true;
        Debug.Log("damageMult"+ damageMult);
        this.damageMultiplier = damageMult;
        this.manaMultiplier = manaMult;
    }
    public override Spell Application(Spell spell) {
        spell.final_damage = Mathf.RoundToInt((spell.final_damage * damageMultiplier));
        spell.final_mana_cost = Mathf.RoundToInt(spell.final_mana_cost * manaMultiplier);
        return spell;
    }
}
