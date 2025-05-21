using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class CharacterSelector : MonoBehaviour
{
    public SpriteView spriteView1;
    public SpriteView spriteView2;
    public SpriteView spriteView3;


    public TextMeshProUGUI ChImfomation1;
    public TextMeshProUGUI ChImfomation2;
    public TextMeshProUGUI ChImfomation3;
    public GameObject DifficultSelector;
    public PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        spriteView1.Apply("mage", GameManager.Instance.playerSpriteManager.Get(0));
        spriteView2.Apply("warlock", GameManager.Instance.playerSpriteManager.Get(1));
        spriteView3.Apply("battlemage", GameManager.Instance.playerSpriteManager.Get(2));
        DisplayInfo();
    }


    public void Chosen(int index)
    {
        
        GameManager.Instance.playerSpriteManager.currentIconIndex = index;
        playerController.loadCharacter(index);
        DifficultSelector.SetActive(true);
        gameObject.SetActive(false);
    }
    public void DisplayInfo()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("classes");
        if (jsonText == null)
        {
            Debug.LogError("classes.json not found in Resources!");
            return;
        }

        var root = Newtonsoft.Json.Linq.JObject.Parse(jsonText.text);

        // Get current wave and default spell power context
        int wave = GameManager.Instance.currentWave;

        string Format(JObject data)
        {
            string health = data["health"]?.ToString();
            string mana = data["mana"]?.ToString();
            string manaReg = data["mana_regeneration"]?.ToString();
            string spellPower = data["spellpower"]?.ToString();
            string speed = data["speed"]?.ToString();

            string result = "";
            result += $"Health: {RPNCalculator.EvaluateFloat(health, wave)}\n";
            result += $"Mana: {RPNCalculator.EvaluateFloat(mana, wave)}\n";
            result += $"Mana Regen: {RPNCalculator.EvaluateFloat(manaReg, wave)}\n";
            result += $"Spell Power: {RPNCalculator.EvaluateFloat(spellPower, wave)}\n";
            result += $"Speed: {RPNCalculator.EvaluateFloat(speed, wave)}\n";
            return result;
        }

        ChImfomation1.text = Format((JObject)root["mage"]);
        ChImfomation2.text = Format((JObject)root["warlock"]);
        ChImfomation3.text = Format((JObject)root["battlemage"]);
    }



}
