
// DoublerModifier.cs
using UnityEngine;
using System.Collections;

public class DoublerModifier : ModifierSpell
{
    private float delay;
    private float manaMultiplier;
    private float cooldownMultiplier;

    public DoublerModifier(float delay, float manaMult, float cooldownMult)
    {
        this.delay = delay;
        this.manaMultiplier = manaMult;
        this.cooldownMultiplier = cooldownMult;
    }

    public override Spell Application(Spell spell)
    {
        spell.final_mana_cost = Mathf.RoundToInt(spell.final_mana_cost * manaMultiplier);
        spell.final_cooldown *= cooldownMultiplier;
        return spell;
    }

    public override IEnumerator CastWithCoroutine(Spell spell, Vector3 where, Vector3 target)
    {
        yield return new WaitForSeconds(delay);
        yield return spell.Cast(where, target, spell.team);
    }
    
}
