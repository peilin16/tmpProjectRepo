using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Spell
{
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;
    public SpellData data;
    public List<ModifierSpell> modifierSpells = new List<ModifierSpell>();


    protected bool is_applicated = false;
    public int final_mana_cost;


    public Vector3 where;
    public Vector3 target;
    public float final_damage;
    public float final_cooldown;
    public float final_secondary_damage;
    public float final_speed;
    public string final_trajectory;
    public float final_life_time;
    public bool castModified = true;
    public bool onHitModified = true;
    public float spellPower;

    public Spell(SpellCaster owner, SpellData data)
    {
        this.owner = owner;
        this.data = data;

        this.final_mana_cost = data.base_mana_cost;
        this.final_damage = data.base_damage;
        this.final_cooldown = data.base_cooldown;
        this.final_speed = data.projectile.base_speed;
        this.final_secondary_damage = data.base_secondary_damage;
        this.final_trajectory = data.projectile.trajectory;
        this.final_life_time = data.projectile.base_lifetime;
    }

    public string GetName()
    {
        return data.name;
    }

    public int GetManaCost()
    {

        return this.final_mana_cost;
    }
    public float GetSpeed()
    {
        return this.final_speed;
    }
    public float GetDamage()
    {
        return this.final_damage;
    }

    public float GetCooldown()
    {
        return this.final_cooldown;
    }

    public int GetIcon()
    {
        return data.icon;
    }
    public string GetDescription()
    {
        return data.description;
    }

    public float getSecondDamage()
    {
        return this.final_secondary_damage;
    }
    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }
    public void applicateModify()
    {
        this.is_applicated = true;
        foreach (var modifier in modifierSpells)
            modifier.Application(this);
    }
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, bool isModified = true)
    {

        this.team = team;
        if (this.is_applicated == false && isModified)
            applicateModify();
       
        if (isModified)
        {
            int i = 0;
            foreach (var modifier in modifierSpells)
            {
                modifier.Cast(this);
                CoroutineManager.Instance.StartManagedCoroutine("Player_spell", modifier.name + i, modifier.CastWithCoroutine(this));
                i += 1;

            }
        }
        
           
        int spriteIndex = int.Parse(data.projectile.sprite);
        float speed = this.final_speed;
        string traj = this.final_trajectory;
        float lifetime = this.final_life_time;


        if (!string.IsNullOrEmpty(data.projectile.lifetime))
        {
            GameManager.Instance.projectileManager.CreateProjectile(spriteIndex, traj, where, target - where, speed, OnHit, lifetime);
        }
        else
        {
            GameManager.Instance.projectileManager.CreateProjectile(spriteIndex, traj, where, target - where, speed, OnHit);
        }
        
        yield return new WaitForEndOfFrame();
    }
    public void ApplyFlatSpellpowerBoost(int bonus)
    {
        final_damage += bonus;
    }


    public void OnHit(Controller other, Vector3 impact)
    {
        //Debug.Log("on hit");
        if (onHitModified)
        {
            int i = 0;
            foreach (var modifier in modifierSpells)
            {
                modifier.Cast(this);
                CoroutineManager.Instance.StartManagedCoroutine("Player_spell", modifier.name + i, modifier.OnHitWithCoroutine(this, other));
                i += 1;

            }
        }
        if (other.character.hp.team != team)
        {
            // Defaulting to arcane damage type, but can be extended to use data.damage.type
            foreach (var modifier in modifierSpells)
                modifier.OnHit(this, other);
            other.character.hp.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
        }
        if (data.secondary_projectile != null)
        {
            int count = data.N_value;
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

                GameManager.Instance.projectileManager.CreateProjectile(
                    int.Parse(data.secondary_projectile.sprite),
                    data.secondary_projectile.trajectory,
                    impact,
                    dir,
                    data.secondary_projectile.base_speed,
                    (Controller other, Vector3 _) =>
                    {
                        if (other.character.hp.team != team)
                            other.character.hp.Damage(new Damage(data.base_secondary_damage, Damage.Type.ARCANE));
                    },
                    data.secondary_projectile.base_lifetime
                );
            }
        }


    }
}
