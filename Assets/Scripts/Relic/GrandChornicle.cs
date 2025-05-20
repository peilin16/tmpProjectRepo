using UnityEngine;
using System.Collections;

public class GrandChronicle : Relic
{
    private float originalSpellPower;
    private bool isEffectActive = false;
    private PlayerController currentPC;

    public override void Application(PlayerController pc)
    {
        currentPC = pc;
        originalSpellPower = pc.player.spellcaster.spellPower;

        // 订阅受伤事件
        EventBus.Instance.OnPlayerDamaged += CheckHealthCondition;
    }

    private void CheckHealthCondition(Damage damage)
    {
        if (currentPC == null || currentPC.player == null) return;

        float currentHealthPercent = currentPC.player.hp.hp * 1f / currentPC.player.hp.max_hp;

        // 血量低于25%且效果未激活
        if (!isEffectActive && currentHealthPercent < 0.25f)
        {
            ActivateEffect();
        }
        // 血量恢复至25%以上且效果已激活
        else if (isEffectActive && currentHealthPercent >= 0.25f)
        {
            DeactivateEffect();
        }
    }

    private void ActivateEffect()
    {
        if (currentPC.player.spellcaster == null) return;

        isEffectActive = true;
        currentPC.player.spellcaster.spellPower = originalSpellPower + amount;
        Debug.Log($"Grand Chronicle: Spell Power +{amount} (Now: {currentPC.player.spellcaster.spellPower})");
    }

    private void DeactivateEffect()
    {
        if (currentPC.player.spellcaster == null) return;

        isEffectActive = false;
        currentPC.player.spellcaster.spellPower = originalSpellPower;
        Debug.Log("Grand Chronicle: Spell Power restored");
    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnPlayerDamaged -= CheckHealthCondition;

        // 移除时恢复原始数值
        if (isEffectActive)
        {
            DeactivateEffect();
        }
    }
}