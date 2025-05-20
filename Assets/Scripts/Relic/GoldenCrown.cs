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

        // 订阅击杀事件
        EventBus.Instance.OnMonsterDeath += OnEnemyKilled;
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        if (currentPC == null || currentPC.player?.spellcaster == null) return;

        // 停止现有的加成协程（如果存在）
        if (powerBoostCoroutine != null)
        {
            CoroutineManager.Instance.StopManagedCoroutine(
                "GoldenCrown",
                currentPC.playerID
            );
        }

        // 应用法术强度加成
        currentPC.player.spellcaster.spellPower = originalSpellPower + amount;
        //currentPC.player.spellcaster.resetSpellsData();

        Debug.Log($"Golden Crown: Spell Power +{amount} (Now: {currentPC.player.spellcaster.spellPower})");

        // 启动3秒计时器
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
            // 恢复原始法术强度
            currentPC.player.spellcaster.spellPower = originalSpellPower;
            Debug.Log("Golden Crown: Spell Power restored");
            isEffectActive = false;
        }
    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnMonsterDeath -= OnEnemyKilled;

        // 立即恢复原始数值
        if (isEffectActive && currentPC != null && currentPC.player?.spellcaster != null)
        {
            currentPC.player.spellcaster.spellPower = originalSpellPower;
        }

        // 停止所有协程
        CoroutineManager.Instance.StopGroup("GoldenCrown");
    }
}
