using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
public class GoldenMask:Relic
{
    
    private bool isBuffActive = false;
    private float ifActivePower;
    private float originalSpellPower; 
    public override void JsonInit(JObject jsonObj)
    {
        base.JsonInit(jsonObj);
        // If in future it needs custom fields, handle here
    }


    public override void Application(PlayerController pc)
    {

        EventBus.Instance.OnPlayerDamaged += OnPlayerDamaged;
        originalSpellPower = pc.player.spellcaster.spellPower;
        ifActivePower = pc.player.spellcaster.spellPower += amount;
        EventBus.Instance.PlayerCast += OnPlayerCast;
    }

    private void OnPlayerDamaged(Damage damage)
    {
        if (!isBuffActive)
        {
            isBuffActive = true;
            
            Debug.Log($"Golden Mask activated: +{amount} spellpower");
        }
    }

    private void OnPlayerCast(PlayerController pc)
    {
        if (isBuffActive && pc.player?.spellcaster != null)
        {
            // 1. apply relic

            pc.player.spellcaster.spellPower = ifActivePower;
            //pc.player.spellcaster.resetSpellsData();
            Debug.Log($"Golden Mask: Applied +{amount} spellpower (Now: {pc.player.spellcaster.spellPower})");

            // 2. retreive coroutine
            //pc.StartCoroutine();
            //CoroutineManager.Instance.StartManagedCoroutine("Relic_Effect", this.name, ResetAfterCast(pc));
            // 3. reset
            isBuffActive = false;
        }
        else if(!isBuffActive && pc.player?.spellcaster != null)
        {
            ResetAfterCast(pc);
        }
    }
    /*
    private IEnumerator ResetAfterCast(PlayerController pc)
    {
        
        yield return null;

        // reset damage value
        if (pc.player?.spellcaster != null)
        {
            pc.player.spellcaster.spellPower = originalSpellPower;
            //pc.player.spellcaster.resetSpellsData();
            Debug.Log("Golden Mask: Spellpower restored");
        }
    }*/
    private void ResetAfterCast(PlayerController pc)
    {
        if(pc.player.spellcaster.spellPower != originalSpellPower)
        {
            pc.player.spellcaster.spellPower = originalSpellPower;
            //pc.player.spellcaster.resetSpellsData();
            Debug.Log("Golden Mask: Spellpower restored");
        }

    }

    public override void OnRemoved()
    {
        EventBus.Instance.OnPlayerDamaged -= OnPlayerDamaged;
        EventBus.Instance.PlayerCast -= OnPlayerCast;
    }

}
