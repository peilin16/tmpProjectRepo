using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{

    private float _spellPower;
    private int _mana;
    public int mana
    {
        get => _mana;
        set
        {
            int newValue = Mathf.Min(value, max_mana);
            if (_mana != newValue)
            {
                _mana = newValue;
                OnManaChanged();
            }
        }
    }
    private int _max_mana;
    public int max_mana
    {
        get => _max_mana;
        set
        {
            if (_max_mana != value)
            {
                _max_mana = value;
                //OnMaxManaChanged();
            }
        }
    }
    public int mana_reg;
    public Hittable.Team team;
    public SpellBuilder builder = new SpellBuilder();
    public List<Spell> spells;

    public PlayerController playerController;
    public float spellPower
    {
        get => _spellPower;
        set
        {
            if (_spellPower != value)
            {
                _spellPower = value;
                OnPowerChanged();
            }
        }
    }




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

        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;

        Debug.Log(" mana:" + this.mana + " power:" + this.spellPower + " max_mana:" + this.max_mana );
        spells = new List<Spell>();
        this.spellPower = power;
        this.mana = mana;
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
    private void OnPowerChanged()
    {
        int wave = GameManager.Instance.currentWave;

        foreach (Spell spell in spells)
        {
            // 只更新与法术强度相关的属性
            if (spell.data.damage != null && !string.IsNullOrEmpty(spell.data.damage.amount))
            {
                spell.final_damage = RPNCalculator.EvaluateFloat(
                    spell.data.damage.amount,
                    wave,
                    this._spellPower
                );
            }

           // Debug.Log($"Power Updated: {spell.GetName()} | " +    $"New Damage: {spell.final_damage}");
        }
    }

    private void OnManaChanged()
    {
        int wave = GameManager.Instance.currentWave;

        foreach (Spell spell in spells)
        {
            // 只更新与法力消耗相关的属性
            if (!string.IsNullOrEmpty(spell.data.mana_cost))
            {
                spell.final_mana_cost = Mathf.FloorToInt(
                    RPNCalculator.EvaluateFloat(
                        spell.data.mana_cost,
                        wave,
                        this._spellPower
                    )
                );
            }

            //Debug.Log($"Mana Updated: {spell.GetName()} | " + $"New Mana Cost: {spell.final_mana_cost}");
        }
    }
}
