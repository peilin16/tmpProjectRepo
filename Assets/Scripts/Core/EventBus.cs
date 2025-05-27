using UnityEngine;
using System;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    //player event
    public Action<Damage> OnPlayerDamaged;
    public Action<GameObject> OnPlayerDeath;
    public Action<PlayerController> PlayerCast; 

    // monster event
    public  Action<Damage, GameObject> OnMonsterDamaged; 
    public  Action<GameObject> OnMonsterDeath;

    // 修改 OnPlayerStandStill 定义（移除 PlayerController 参数）
    public Action OnPlayerStandStill;  // 改为无参数事件

    // 添加静止时间字段（如果必须保留）
    public float standStillTime { get; private set; }

    public void TriggerPlayerCast(PlayerController pc) => PlayerCast?.Invoke(pc);

    // 添加触发方法
    public void TriggerStandStill()
    {
        OnPlayerStandStill?.Invoke();
        standStillTime = 0f; // 触发后重置计时
    }


    // 通用物理伤害事件（保留原始设计）
    public event Action<Vector3, Damage, Hittable> OnPhysicalDamage;

    public void TriggerPhysicalDamage(Vector3 position, Damage damage, Hittable target)
    {
        OnPhysicalDamage?.Invoke(position, damage, target);
    }
    // 在 EventBus 类中添加
    public void TriggerPlayerDamaged(Damage damage)
    {
        OnPlayerDamaged?.Invoke(damage);
    }

    public void TriggerOnMonsterDamaged(Damage damage, GameObject monster)
    {
        OnMonsterDamaged?.Invoke(damage,monster);
    }
    public void TriggerOnMonsterDeath(GameObject monster)
    {
        OnMonsterDeath?.Invoke(monster);
    }
   public void TriggerOnPlayerDeath(GameObject player)
    {
        if(GameManager.Instance.state == GameManager.GameState.INWAVE)
            OnPlayerDeath?.Invoke(player);
    }


}
