using UnityEngine;
using System.Collections;

public class KnightShield : Relic
{
    private bool isActive = false;
    private Coroutine recoveryCoroutine;
    private PlayerController currentPC;
    private float recoveryThreshold; // 15%阈值
    private float recoveryRate; // 恢复速率

    public override void Application(PlayerController pc)
    {
        currentPC = pc;
        recoveryRate = amount; 
        recoveryThreshold = pc.player.hp.max_hp * 0.15f; // 计算15%阈值

        EventBus.Instance.OnPlayerDamaged += CheckHealthCondition;
    }

    private void CheckHealthCondition(Damage damage)
    {
        if (currentPC == null || currentPC.player == null) return;

        // 当前血量百分比
        float currentHealthPercent = currentPC.player.hp.hp * 1f / currentPC.player.hp.max_hp;

        // 条件：血量<15%且未激活
        if (!isActive && currentHealthPercent < 0.15f)
        {
            isActive = true;
            recoveryCoroutine = CoroutineManager.Instance.StartManagedCoroutine(
                "KnightShield",
                currentPC.playerID,
                HealthRecoveryProcess()
            );
        }
        // 条件：血量≥15%或玩家死亡时停止
        else if (isActive && (currentPC.player.hp.hp >= (int)recoveryThreshold || currentPC.player.hp.hp <= 0))
        {
            StopRecovery();
        }
    }

    private IEnumerator HealthRecoveryProcess()
    {
        while (isActive && currentPC != null && currentPC.player != null)
        {
           
            // 计算本次恢复量(最大血量的0.3%)
            float healAmount = currentPC.player.hp.max_hp * recoveryRate;

            // 确保不超过15%阈值
            float targetHealth = Mathf.Min(
                currentPC.player.hp.hp + healAmount,
                recoveryThreshold
            );

            currentPC.player.hp.hp = Mathf.RoundToInt(targetHealth);

            // 更新血条UI
            if (currentPC.healthui != null)
            {
                currentPC.healthui.SetHealth(currentPC.player.hp);
            }
            Debug.Log("KnightShield Trigger: Heal " + currentPC.player.hp.hp + " recovery " + recoveryThreshold +" target "+ targetHealth);
            // 检查是否达到终止条件
            /*if (currentPC.player.hp.hp >= (int)recoveryThreshold || currentPC.player.hp.hp <= 0)
            {
                StopRecovery();
                Debug.Log("KnightShield Inactive");
                yield break;
            }*/

            yield return new WaitForSeconds(1f); // 每秒恢复一次
        }
    }

    private void StopRecovery()
    {
        if (isActive)
        {
            Debug.Log("KnightShield Inactive");
            isActive = false;
            currentPC.player.hp.hp += (int)(currentPC.player.hp.max_hp * 0.01f);
            CoroutineManager.Instance.StopManagedCoroutine(
                "KnightShield",
                currentPC.playerID
            );
        }
    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnPlayerDamaged -= CheckHealthCondition;
        StopRecovery();
    }
}