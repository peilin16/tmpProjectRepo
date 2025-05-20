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

        // ���������¼�
        EventBus.Instance.OnPlayerDamaged += CheckHealthCondition;
    }

    private void CheckHealthCondition(Damage damage)
    {
        if (currentPC == null || currentPC.player == null) return;

        float currentHealthPercent = currentPC.player.hp.hp * 1f / currentPC.player.hp.max_hp;

        // Ѫ������25%��Ч��δ����
        if (!isEffectActive && currentHealthPercent < 0.25f)
        {
            ActivateEffect();
        }
        // Ѫ���ָ���25%������Ч���Ѽ���
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

        // �Ƴ�ʱ�ָ�ԭʼ��ֵ
        if (isEffectActive)
        {
            DeactivateEffect();
        }
    }
}