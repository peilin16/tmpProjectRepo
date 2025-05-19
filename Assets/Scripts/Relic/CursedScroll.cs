using UnityEngine;

public class CursedScroll:Relic
{
    public override void Application(PlayerController pc)
    {
        // ¶©ÔÄµĞÈËËÀÍöÊÂ¼ş
        EventBus.Instance.OnMonsterDeath += OnEnemyKilled;
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        if (GameManager.Instance.player.TryGetComponent(out PlayerController pc))
        {
            pc.player.spellcaster.mana += Mathf.RoundToInt(amount);
            pc.manaui?.UpdateMana(pc.player.spellcaster.mana, pc.player.spellcaster.max_mana);
            Debug.Log($"Cursed Scroll: Gained {amount} mana");
        }
    }

    public override void OnRemoved()
    {
        //EventBus.Instance.OnEnemyKilled -= OnEnemyKilled;
    }
}
