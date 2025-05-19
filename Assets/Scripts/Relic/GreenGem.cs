using UnityEngine;

public class GreenGem:Relic
{
    private bool isHooked = false;

    public override void Application(PlayerController pc)
    {
        if (!isHooked && pc.player != null && pc.player.hp != null)
        {

            EventBus.Instance.OnPlayerDamaged += (damage) => OnPlayerDamaged(pc, damage);
            isHooked = true;
        }
    }
    private void OnPlayerDamaged(PlayerController pc, Damage damage)
    {

        //if (pc.player != null && pc.player.spellcaster != null)
        //{
        pc.player.spellcaster.mana += Mathf.RoundToInt(amount); 
        Debug.Log($"GreenGem: Restored {amount} mana after taking damage");
        //}
    }
}
