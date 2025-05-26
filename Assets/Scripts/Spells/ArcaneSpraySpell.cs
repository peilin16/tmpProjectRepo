using UnityEngine;

using System.Collections;

public class ArcaneSpraySpell : Spell
{

    public ArcaneSpraySpell(SpellCaster owner, SpellData data) : base(owner, data)
    {

    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, bool isModified = true)
    {
        this.team = team;

        // Recalculate final values (base + modifiers)
        //calculateFinalData();
        if (this.is_applicated == false &&isModified)
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
        Vector3 direction = (target - where).normalized;

        int count = data.N_value;
        float coneAngle = 60f; // Hardcoded cone angle
        float halfCone = coneAngle / 2f;

        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            float t = (count == 1) ? 0 : i / (float)(count - 1);
            float angleOffset = Mathf.Lerp(-halfCone, halfCone, t);
            float angle = baseAngle + angleOffset;
            Vector3 sprayDir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

            GameManager.Instance.projectileManager.CreateProjectile(
                int.Parse(data.projectile.sprite),
                final_trajectory,
                where,
                sprayDir,
                final_speed,
                (Controller other, Vector3 _) =>
                {
                    if (other.character.hp.team != team)
                        other.character.hp.Damage(new Damage(this.final_damage, Damage.Type.ARCANE));
                },
                final_life_time
            );
        }

        yield return new WaitForEndOfFrame();
    }
}