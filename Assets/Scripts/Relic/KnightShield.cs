using UnityEngine;
using System.Collections;

public class KnightShield : Relic
{
    private bool isActive = false;
    private Coroutine recoveryCoroutine;
    private PlayerController currentPC;
    private float recoveryThreshold; // 15%��ֵ
    private float recoveryRate; // �ָ�����

    public override void Application(PlayerController pc)
    {
        currentPC = pc;
        recoveryRate = amount; 
        recoveryThreshold = pc.player.hp.max_hp * 0.15f; // ����15%��ֵ

        EventBus.Instance.OnPlayerDamaged += CheckHealthCondition;
    }

    private void CheckHealthCondition(Damage damage)
    {
        if (currentPC == null || currentPC.player == null) return;

        // ��ǰѪ���ٷֱ�
        float currentHealthPercent = currentPC.player.hp.hp * 1f / currentPC.player.hp.max_hp;

        // ������Ѫ��<15%��δ����
        if (!isActive && currentHealthPercent < 0.15f)
        {
            isActive = true;
            recoveryCoroutine = CoroutineManager.Instance.StartManagedCoroutine(
                "KnightShield",
                currentPC.playerID,
                HealthRecoveryProcess()
            );
        }
        // ������Ѫ����15%���������ʱֹͣ
        else if (isActive && (currentPC.player.hp.hp >= (int)recoveryThreshold || currentPC.player.hp.hp <= 0))
        {
            StopRecovery();
        }
    }

    private IEnumerator HealthRecoveryProcess()
    {
        while (isActive && currentPC != null && currentPC.player != null)
        {
           
            // ���㱾�λָ���(���Ѫ����0.3%)
            float healAmount = currentPC.player.hp.max_hp * recoveryRate;

            // ȷ��������15%��ֵ
            float targetHealth = Mathf.Min(
                currentPC.player.hp.hp + healAmount,
                recoveryThreshold
            );

            currentPC.player.hp.hp = Mathf.RoundToInt(targetHealth);

            // ����Ѫ��UI
            if (currentPC.healthui != null)
            {
                currentPC.healthui.SetHealth(currentPC.player.hp);
            }
            Debug.Log("KnightShield Trigger: Heal " + currentPC.player.hp.hp + " recovery " + recoveryThreshold +" target "+ targetHealth);
            // ����Ƿ�ﵽ��ֹ����
            /*if (currentPC.player.hp.hp >= (int)recoveryThreshold || currentPC.player.hp.hp <= 0)
            {
                StopRecovery();
                Debug.Log("KnightShield Inactive");
                yield break;
            }*/

            yield return new WaitForSeconds(1f); // ÿ��ָ�һ��
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