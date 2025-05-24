
using UnityEngine;
using System.Collections;

public class MysteriousMask : Relic
{
    private int originalSpeed;
    private string coroutineId; // ���ڱ�ʶЭ��
    private int additionalSpeed;
    private int ifActiveSpeed;
    public override void Application(PlayerController pc)
    {
        EventBus.Instance.OnMonsterDeath += OnEnemyKilled;
        originalSpeed = pc.player.speed;
        
        coroutineId = "1";// pc.GetInstanceID().ToString(); // ʹ�����ʵ��ID��ΪΨһ��ʶ
        additionalSpeed = (int)this.amount;
        ifActiveSpeed = pc.player.speed + additionalSpeed;

    }

    private void OnEnemyKilled(GameObject enemy)
    {
        if (!(GameManager.Instance.player.TryGetComponent(out PlayerController pc)))
            return;

        // ֹ֮ͣǰ��Э�̣�������ڣ�
        CoroutineManager.Instance.StopManagedCoroutine("MysteriousMask", coroutineId);

        // Ӧ���ٶȼӳ�
        pc.player.speed = ifActiveSpeed;
        Debug.Log($"Mysterious Mask: Speed +{additionalSpeed} (Now: {pc.player.speed})");

        // ����3������õ�Э��
        CoroutineManager.Instance.StartManagedCoroutine(
            "MysteriousMask",
            coroutineId,
            ResetSpeedAfterDelay(pc, 3f)
        );
    }

    private IEnumerator ResetSpeedAfterDelay(PlayerController pc, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (pc != null && pc.player != null)
        {
            pc.player.speed = originalSpeed;
            Debug.Log("Mysterious Mask: Speed restored");
        }
    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnMonsterDeath -= OnEnemyKilled;

        // ֹͣ�������Э�̲��ָ��ٶ�
        CoroutineManager.Instance.StopGroup("MysteriousMask");

        if (GameManager.Instance.player.TryGetComponent(out PlayerController pc))
        {
            pc.player.speed = originalSpeed;
        }
    }
}