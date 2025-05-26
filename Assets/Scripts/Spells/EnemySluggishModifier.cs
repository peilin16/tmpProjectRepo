using UnityEngine;

public class EnemySluggishModifier: ModifierSpell
{
    private float speedMultiplier = 0.5f;
    private float duration = 5f;

    public EnemySluggishModifier() { }

    public EnemySluggishModifier(float speedMultiplier, float duration)
    {
        this.speedMultiplier = speedMultiplier;
        this.duration = duration;
    }

    public override Spell OnHit(Spell spell, Controller other)
    {
        // Wrap the existing OnHit behavior to add the slow effect

            // Only apply if target is an enemy
        /*if (other.team != spell.team)
        {
                // Apply slow effect if method exists on enemy
            if (other is Enemy enemy)
            {
                enemy.ApplySpeedMultiplier(speedMultiplier, duration);
            }
        }*/
        

        // Override OnHit behavior
        
        return spell;
    }
}
