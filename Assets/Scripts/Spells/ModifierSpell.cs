using System.Collections;  // <== THIS is required for IEnumerator
using UnityEngine;

public abstract class ModifierSpell
{
    public string name;
    public string description;
    public bool one_time = false;
    public virtual Spell Cast(Spell spell) { return spell; }
    public virtual Spell OnHit(Spell spell) { return spell; }
    public virtual Spell Application(Spell spell) { return spell; }
    public virtual IEnumerator CastWithCoroutine(Spell spell, Vector3 where, Vector3 target)
    {
        yield break; 
    }
}