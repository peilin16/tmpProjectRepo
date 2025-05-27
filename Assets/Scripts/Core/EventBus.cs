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

    // �޸� OnPlayerStandStill ���壨�Ƴ� PlayerController ������
    public Action OnPlayerStandStill;  // ��Ϊ�޲����¼�

    // ��Ӿ�ֹʱ���ֶΣ�������뱣����
    public float standStillTime { get; private set; }

    public void TriggerPlayerCast(PlayerController pc) => PlayerCast?.Invoke(pc);

    // ��Ӵ�������
    public void TriggerStandStill()
    {
        OnPlayerStandStill?.Invoke();
        standStillTime = 0f; // ���������ü�ʱ
    }


    // ͨ�������˺��¼�������ԭʼ��ƣ�
    public event Action<Vector3, Damage, Hittable> OnPhysicalDamage;

    public void TriggerPhysicalDamage(Vector3 position, Damage damage, Hittable target)
    {
        OnPhysicalDamage?.Invoke(position, damage, target);
    }
    // �� EventBus �������
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
