using UnityEngine;
using System;

public class Hittable
{

    public enum Team { PLAYER, MONSTERS }
    public Team team;

    public int hp;
    public int max_hp;

    public GameObject owner;

    public void Damage(Damage damage)
    {

        EventBus.Instance.TriggerPhysicalDamage(owner.transform.position, damage, this);

        // 扣血逻辑
        hp -= (int)Math.Round(damage.amount);

        // 根据队伍触发不同事件
        switch (team)
        {
            case Team.PLAYER:
                EventBus.Instance.TriggerPlayerDamaged(damage);

                break;
            case Team.MONSTERS:
                EventBus.Instance.TriggerOnMonsterDamaged(damage, owner);
                break;
        }

        // die
        if (hp <= 0)
        {
            hp = 0;
            if (team == Team.PLAYER)
                EventBus.Instance.TriggerOnPlayerDeath(owner);
            else
                EventBus.Instance.TriggerOnMonsterDeath(owner);
        }
    }



    public Hittable(int hp, Team team, GameObject owner)
    {
        this.hp = hp;
        this.max_hp = hp;
        this.team = team;
        this.owner = owner;
    }

    public void SetMaxHP(int max_hp)
    {
        float perc = this.hp * 1.0f / this.max_hp;
        this.max_hp = max_hp;
        this.hp = Mathf.RoundToInt(perc * max_hp);
    }
}
