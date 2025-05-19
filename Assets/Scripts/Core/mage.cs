using UnityEngine;
using Newtonsoft.Json.Linq;
public class Mage// : PlayerCharacter
{


    public  void StartLevel()
    {
        /*spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());*/


        // Load Mage class stats
        /*TextAsset jsonText = Resources.Load<TextAsset>("classes");
        if (jsonText == null)
        {
            Debug.LogError("classes.json not found in Resources!");
            return;
        }

        JObject root = JObject.Parse(jsonText.text);
        JObject mageStats = (JObject)root["mage"];
        int wave = GameManager.Instance.maxWaves;
        int power = 10;

        // Calculate base stats using RPN expressions
        float health = RPNCalculator.EvaluateFloat(mageStats["health"].ToString(), wave, power);
        float mana = RPNCalculator.EvaluateFloat(mageStats["mana"].ToString(), wave, power);
        float manaReg = RPNCalculator.EvaluateFloat(mageStats["mana_regeneration"].ToString(), wave, power);
        float spellPower = RPNCalculator.EvaluateFloat(mageStats["spellpower"].ToString(), wave, power);
        float moveSpeed = RPNCalculator.EvaluateFloat(mageStats["speed"].ToString(), wave, power);

        // Assign speed
        speed = Mathf.RoundToInt(moveSpeed);

        // Set up spellcaster
        spellcaster = new SpellCaster(Mathf.RoundToInt(mana), Mathf.RoundToInt(manaReg), Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());

        // Set up health
        hp = new Hittable(Mathf.RoundToInt(health), Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += base.Die;

        // Set up UI
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);

        Debug.Log($"Mage initialized with {health} HP, {mana} Mana, {manaReg}/s regen, {spellPower} spell power, speed {moveSpeed}");*/

    }
}
