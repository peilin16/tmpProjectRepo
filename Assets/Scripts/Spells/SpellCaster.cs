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

    public PlayerController playerController;
    public float spellPower;
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


    public SpellCaster(int mana, int mana_reg, Hittable.Team team, float power)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.spellPower = power;
        Debug.Log(" mana:" + this.mana + " power:" + this.spellPower + " max_mana:" + this.max_mana );
        spells = new List<Spell>();
        Spell inner = builder.Build(this, "arcane_bolt");
        spells.Add(inner);
        //spells.Add(builder.MakeRandomSpell(this));
        //spells[0].applicateModify();

        /*spells[1] =builder.MakeRandomSpell(this);
        spells[2] = builder.MakeRandomSpell(this);
        spells[3] = builder.MakeRandomSpell(this);*/
        Debug.Log(" spell:" + inner.data.name + " damage:" + inner.GetDamage() + " cooldown:" + inner.GetCooldown() + " speed:" + inner.GetSpeed() );


    }
    public IEnumerator Cast(Vector3 where, Vector3 target, int spellIndex = 0, bool isModified = true)
    {
        EventBus.Instance.TriggerPlayerCast(playerController);



        this.current_where = where;
        this.current_target = target;
        Spell spell = spells[spellIndex];
        spell.where = where;
        spell.target = target;
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            yield return spell.Cast(where, target, team);
        }

        Debug.Log($"Spell Cast:[Spell Reset] {spell.GetName()} - Damage: {spell.final_damage},  -Base Damage: {spell.data.base_damage}, Mana: {spell.final_mana_cost}");
        //OnSpellCast?.Invoke(spell);
        yield break;
    }
    public void resetSpellsData()
    {
        int wave = GameManager.Instance.currentWave;

        foreach (Spell spell in spells)
        {
            if (spell.data.damage != null && !string.IsNullOrEmpty(spell.data.damage.amount))
            {
                spell.final_damage =RPNCalculator.EvaluateFloat(spell.data.damage.amount, wave, this.spellPower);
            }

            // Optional: re-evaluate cooldown, speed, mana if relics can modify them too
            /*if (!string.IsNullOrEmpty(spell.data.cooldown))
            {
                spell.data.final_cooldown = RPNCalculator.EvaluateFloat(spell.data.cooldown, wave, Mathf.RoundToInt(this.spellPower));
            }

            if (!string.IsNullOrEmpty(spell.data.mana_cost))
            {
                spell.data.final_mana_cost = Mathf.FloorToInt(
                    RPNCalculator.EvaluateFloat(spell.data.mana_cost, wave, Mathf.RoundToInt(this.spellPower))
                );
            }

            if (spell.data.projectile != null && !string.IsNullOrEmpty(spell.data.projectile.speed))
            {
                spell.data.projectile.final_speed = RPNCalculator.EvaluateFloat(
                    spell.data.projectile.speed, wave, Mathf.RoundToInt(this.spellPower));
            }*/

            Debug.Log($"Reset Method Call:[Spell Reset] {spell.GetName()} - Damage: {spell.final_damage}, Cooldown: {spell.final_cooldown}, Mana: {spell.final_mana_cost}");
        }
    }
}
