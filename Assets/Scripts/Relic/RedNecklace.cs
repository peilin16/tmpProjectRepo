using UnityEngine;
using System.Collections;



public class RedNecklace : Relic
{
    private float originPower;
    private float modifiedPower;
    public override void StartWave(PlayerController pc)
    {
        CoroutineManager.Instance.StartManagedCoroutine("player_relic_buff", "RedNecklace", ApplySpellpowerBuff(pc));
    }

    private IEnumerator ApplySpellpowerBuff(PlayerController pc)
    {
        Debug.Log("Red Necklace trigger");
        originPower = pc.player.spellcaster.spellPower;
        modifiedPower = originPower + amount;
        pc.player.spellcaster.spellPower = modifiedPower;


        yield return new WaitForSeconds(10f); // duration from "until": "duration 10"
        Debug.Log("Red Necklace untrigger");
        pc.player.spellcaster.spellPower = originPower;
        
    }
}