using System.Collections;  // <== THIS is required for IEnumerator
using UnityEngine;

public abstract class ModifierSpell
{
    public string name;
    public string description;
    public bool one_time = false;
    public virtual Spell Cast(Spell spell) { return spell; }
    public virtual Spell OnHit(Spell spell, Controller other) { return spell; }
    public virtual Spell Application(Spell spell) { return spell; }
    public virtual IEnumerator CastWithCoroutine(Spell spell)
    {
        yield break; 
    }
    public virtual IEnumerator OnHitWithCoroutine(Spell spell, Controller other)
    {
        yield break;
    }

}