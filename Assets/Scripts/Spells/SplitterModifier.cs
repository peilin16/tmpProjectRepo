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
    
    public override IEnumerator CastWithCoroutine(Spell spell, Vector3 where, Vector3 target)
    {
        Vector3 direction = (target - where).normalized;

        Vector3 rotatedLeft = Quaternion.Euler(0, 0, angle) * direction;
        Vector3 rotatedRight = Quaternion.Euler(0, 0, -angle) * direction;

        yield return spell.Cast(where, where + rotatedLeft, spell.team);
        yield return spell.Cast(where, where + rotatedRight, spell.team);
    }


}