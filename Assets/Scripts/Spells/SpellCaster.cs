using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public SpellBuilder builder = new SpellBuilder();
    public List<Spell> spells;



    public Vector3 current_where;
    public Vector3 current_target;
    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }
    /*
    public SpellCaster(int mana, int mana_reg, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        spell = new SpellBuilder().Build(this);
    }*/


    public SpellCaster(int mana, int mana_reg, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;

        
        spells = new List<Spell>();
        //spells.Add(builder.Build(this, "arcane_bolt"));
        spells.Add(builder.MakeRandomSpell(this));
        //spells[0].applicateModify();

        /*spells[1] =builder.MakeRandomSpell(this);
        spells[2] = builder.MakeRandomSpell(this);
        spells[3] = builder.MakeRandomSpell(this);*/
        Spell inner = spells[0];
        Debug.Log(" spell:" + inner.data.name + " damage:" + inner.GetDamage() + " cooldown:" + inner.GetCooldown() + " speed:" + inner.GetSpeed() );


    }
    public IEnumerator Cast(Vector3 where, Vector3 target, int spellIndex = 0)
    {
        this.current_where = where;
        this.current_target = target;
        Spell spell = spells[spellIndex];
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            yield return spell.Cast(where, target, team);
        }
        yield break;
    }
    public IEnumerator modifierCast(Vector3 where, Vector3 target, int spellIndex = 0)
    {
        Spell spell = spells[spellIndex];
        foreach (var modifier in spell.modifierSpells)
        {
            
            yield return modifier.CastWithCoroutine(spell, where, target);
            
        }
    }
   
    /*
    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            yield return spell.Cast(where, target, team);
        }
        yield break;
    }
    */
}
