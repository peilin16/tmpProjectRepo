using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RelicUI : MonoBehaviour
{
    public SpriteView spriteView1;
    public SpriteView spriteView2;
    public SpriteView spriteView3;
    public TextMeshProUGUI RelicImfomation1;
    public TextMeshProUGUI RelicImfomation2;
    public TextMeshProUGUI RelicImfomation3;
    public List<Relic> displayRelic = new List<Relic>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Static list of available relic names
    private static readonly string[] allRelicNames = {
        "Green Gem", "Jade Elephant", "Golden Mask", "Cursed Scroll"
    };
    public void Start()
    {
        if(displayRelic.Count > 0)
            displayRelic.Clear();
        List<string> chosenNames = new List<string>();

        // Randomly choose 3 unique relic names
        while (chosenNames.Count < 3)
        {
            string candidate = allRelicNames[Random.Range(0, allRelicNames.Length)];
            if (!chosenNames.Contains(candidate))
                chosenNames.Add(candidate);
        }

        // Fetch relics from manager and populate UI
        for (int i = 0; i < 3; i++)
        {
            var r = GameManager.Instance.relicManager.GetRelic<GreenGem>("Green Gem");
            displayRelic.Add(r);

            // Apply sprite and text
            switch (i)
            {
                case 0:
                    spriteView1.Apply(r.name, GameManager.Instance.relicIconManager.Get(r.iconIndex));
                    RelicImfomation1.text = FormatRelicText(r);
                    break;
                case 1:
                    spriteView2.Apply(r.name, GameManager.Instance.relicIconManager.Get(r.iconIndex));
                    RelicImfomation2.text = FormatRelicText(r);
                    break;
                case 2:
                    spriteView3.Apply(r.name, GameManager.Instance.relicIconManager.Get(r.iconIndex));
                    RelicImfomation3.text = FormatRelicText(r);
                    break;
            }
        }

    }
    
    string FormatRelicText(Relic relic)
    {
        return $"<b>Trigger:</b> {relic.triggerDescription}\n<b>Effect:</b> {relic.effectDescription}";
    }

    public void Chosen(int index)
    {
        if (index < 0 || index >= displayRelic.Count)
        {
            Debug.LogError("Invalid relic selection index.");
            return;
        }

       // Relic selected = displayRelic[index];
        //GameManager.Instance.relicInventory.AddRelic(selected); 

        gameObject.SetActive(false);
    }
}
