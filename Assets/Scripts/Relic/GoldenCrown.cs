using UnityEngine;
using System.Collections;

public class GoldenCrown : Relic
{
    private float originalSpellPower;
    private Coroutine powerBoostCoroutine;
    private PlayerController currentPC;
    private bool isEffectActive = false;

    public override void Application(PlayerController pc)
    {
        currentPC = pc;
        originalSpellPower = pc.player.spellcaster.spellPower;

        // ���Ļ�ɱ�¼�
        EventBus.Instance.OnMonsterDeath += OnEnemyKilled;
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        if (currentPC == null || currentPC.player?.spellcaster == null) return;

        // ֹͣ���еļӳ�Э�̣�������ڣ�
        if (powerBoostCoroutine != null)
        {
            CoroutineManager.Instance.StopManagedCoroutine(
                "GoldenCrown",
                currentPC.playerID
            );
        }

        // Ӧ�÷���ǿ�ȼӳ�
        currentPC.player.spellcaster.spellPower = originalSpellPower + amount;
        //currentPC.player.spellcaster.resetSpellsData();

        Debug.Log($"Golden Crown: Spell Power +{amount} (Now: {currentPC.player.spellcaster.spellPower})");

        // ����3���ʱ��
        powerBoostCoroutine = CoroutineManager.Instance.StartManagedCoroutine(
            "GoldenCrown",
            currentPC.playerID,
            SpellPowerBoostDuration()
        );

        isEffectActive = true;
    }

    private IEnumerator SpellPowerBoostDuration()
    {
        yield return new WaitForSeconds(3f);

        if (isEffectActive && currentPC != null && currentPC.player?.spellcaster != null)
        {
            // �ָ�ԭʼ����ǿ��
            currentPC.player.spellcaster.spellPower = originalSpellPower;
            Debug.Log("Golden Crown: Spell Power restored");
            isEffectActive = false;
        }
    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnMonsterDeath -= OnEnemyKilled;

        // �����ָ�ԭʼ��ֵ
        if (isEffectActive && currentPC != null && currentPC.player?.spellcaster != null)
        {
            currentPC.player.spellcaster.spellPower = originalSpellPower;
        }

        // ֹͣ����Э��
        CoroutineManager.Instance.StopGroup("GoldenCrown");
    }
}
