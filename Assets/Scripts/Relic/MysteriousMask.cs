
using UnityEngine;
using System.Collections;

public class MysteriousMask : Relic
{
    private int originalSpeed;
    private string coroutineId; // 用于标识协程
    private int additionalSpeed;
    private int ifActiveSpeed;
    public override void Application(PlayerController pc)
    {
        EventBus.Instance.OnMonsterDeath += OnEnemyKilled;
        originalSpeed = pc.player.speed;
        
        coroutineId = "1";// pc.GetInstanceID().ToString(); // 使用玩家实例ID作为唯一标识
        additionalSpeed = (int)this.amount;
        ifActiveSpeed = pc.player.speed + additionalSpeed;

    }

    private void OnEnemyKilled(GameObject enemy)
    {
        if (!(GameManager.Instance.player.TryGetComponent(out PlayerController pc)))
            return;

        // 停止之前的协程（如果存在）
        CoroutineManager.Instance.StopManagedCoroutine("MysteriousMask", coroutineId);

        // 应用速度加成
        pc.player.speed = ifActiveSpeed;
        Debug.Log($"Mysterious Mask: Speed +{additionalSpeed} (Now: {pc.player.speed})");

        // 启动3秒后重置的协程
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

        // 停止所有相关协程并恢复速度
        CoroutineManager.Instance.StopGroup("MysteriousMask");

        if (GameManager.Instance.player.TryGetComponent(out PlayerController pc))
        {
            pc.player.speed = originalSpeed;
        }
    }
}