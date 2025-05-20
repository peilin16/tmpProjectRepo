using UnityEngine;

public class ChaosModifier : ModifierSpell
{
    private string damageMultiplierExpr;
    public float multiplier = 0;
    public ChaosModifier(string damageExpr)
    {
        this.damageMultiplierExpr = damageExpr;


    }
    public void setMultiplier()
    {
        this.multiplier = RPNCalculator.EvaluateFloat(damageMultiplierExpr, GameManager.Instance.currentWave);
    }
    public override Spell Application(Spell spell)
    {
        if (multiplier == 0)
            setMultiplier();
        int wave = GameManager.Instance.currentWave;
        //int power = 10;
        //float multiplier = RPNCalculator.EvaluateFloat(damageMultiplierExpr, wave, power);
        spell.final_damage = Mathf.RoundToInt(spell.final_damage * multiplier);

        //if (spell.data.projectile != null)
        spell.final_trajectory = "spiraling";

        return spell;
    }

}
