using UnityEngine;
using System.Collections;  // <== THIS is required for IEnumerator
using Newtonsoft.Json.Linq;

public class Relic
{
    public string name;
    public int iconIndex;
    public string triggerDescription;
    public string effectDescription;
    public float amount;
    public virtual void JsonInit(JObject jsonObj) {
        name = jsonObj["name"]?.ToString();
        iconIndex = jsonObj["sprite"]?.ToObject<int>() ?? 0;
        triggerDescription = jsonObj["trigger"]?["description"]?.ToString();
        effectDescription = jsonObj["effect"]?["description"]?.ToString();

        string amountStr = jsonObj["effect"]?["amount"]?.ToString();
        if (!string.IsNullOrEmpty(amountStr))
        {
            amount = RPNCalculator.EvaluateFloat(amountStr, GameManager.Instance.currentWave, 10);
        }


    }
    public virtual void StartLevel(PlayerController pc) { }
    public virtual IEnumerator RelicCoroutine (PlayerController pc)
    {
        yield break;
    }
    public virtual void Update(PlayerController pc) { }
    public virtual void Application(PlayerController pc) { }
    public virtual void OnRemoved() { }
}
