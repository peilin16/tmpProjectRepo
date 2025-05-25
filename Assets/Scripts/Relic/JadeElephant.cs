using UnityEngine;
using Newtonsoft.Json.Linq;
public class JadeElephant : Relic
{
    public float triggerAmount;
    private bool triggered = false;
    private Vector3 lastPosition;
    //private bool isFirstFrame = true;
    public override void JsonInit(JObject jsonObj)
    {
        base.JsonInit(jsonObj);

        string triggerAmountStr = jsonObj["trigger"]?["amount"]?.ToString();
        if (!string.IsNullOrEmpty(triggerAmountStr))
        {
            triggerAmount = RPNCalculator.EvaluateFloat(triggerAmountStr, GameManager.Instance.currentWave, 10);
        }
    }
    public override void Application(PlayerController pc)
    {

        EventBus.Instance.OnPlayerStandStill += OnStandStill;
    }

    private void OnStandStill() // 移除无用参数
    {
        if (!triggered && GameManager.Instance.player.TryGetComponent(out PlayerController pc))
        {
            Debug.Log($"Jade Elephant: + {pc.player.spellcaster.spellPower} spellpower");
            pc.player.spellcaster.spellPower += amount;
            //pc.player.spellcaster.resetSpellsData();
            triggered = true;
            Debug.Log($"Jade Elephant: + {pc.player.spellcaster.spellPower} spellpower");
        }
    }

    public override void Update(PlayerController pc)
    {
        if (triggered && pc.unit.movement.sqrMagnitude > 0.01f)
        {
            pc.player.spellcaster.spellPower -= amount;
            //pc.player.spellcaster.resetSpellsData();
            triggered = false;
            Debug.Log("Jade Elephant: Bonus removed (player moved)");
        }
    }

    // 在Relic被移除时取消订阅
    public override void OnRemoved()
    {
        EventBus.Instance.OnPlayerStandStill -= OnStandStill;
    }

}
