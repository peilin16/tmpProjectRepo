using System.Collections;
using UnityEngine;

public class SplitterModifier : ModifierSpell
{
    private float angle;
    private float manaMultiplier;

    public SplitterModifier(float angle, float manaMult)
    {
        this.angle = angle;
        this.manaMultiplier = manaMult;
    }

    public override Spell Application(Spell spell)
    {

        spell.final_mana_cost = Mathf.RoundToInt(spell.final_mana_cost * manaMultiplier);
        return spell;
    }
    
    public override IEnumerator CastWithCoroutine(Spell spell)
    {
        Vector3 direction = (spell.target - spell.where).normalized;

        Vector3 rotatedLeft = Quaternion.Euler(0, 0, angle) * direction;
        Vector3 rotatedRight = Quaternion.Euler(0, 0, -angle) * direction;

        yield return spell.Cast(spell.where, spell.where + rotatedLeft, spell.team, false);
        yield return spell.Cast(spell.where, spell.where + rotatedRight, spell.team, false);
    }


}