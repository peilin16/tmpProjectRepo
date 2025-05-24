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

    public PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Static list of available relic names

    public void OnEnable()
    {
        displayRelic.Clear();

        // Get full relic list from RelicManager
        var allRelics = GameManager.Instance.relicManager.GetAllRelics(); 
        var carried = player.carriedRelic;

        // Filter out already carried relics
        List<Relic> available = new List<Relic>();
        foreach (var relic in allRelics)
        {
            if (!player.carriedRelic.Contains(relic))
            {
                available.Add(relic);
            }
        }

        // Shuffle and take 3 unique ones
        for (int i = 0; i < 3 && available.Count > 0; i++)
        {

            int index = Random.Range(0, available.Count);
            Debug.Log("index:" + index + " count:" + available.Count);
            var r = available[index];
            Debug.Log("name:" + r.name);
            available.RemoveAt(index);
            displayRelic.Add(r);

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
    public void RelicUIStart()
    {

        //this.SetActive(true);
    }
    string FormatRelicText(Relic relic)
    {
        return $"<b>Trigger:</b> {relic.triggerDescription}\n<b>Effect:</b> {relic.effectDescription}";
    }

    public void Chosen(int index)
    {
        if (index < 0 || index >=  displayRelic.Count)
        {
            Debug.LogError("Invalid relic selection index.");
            return;
        }
        player.TakeRelic(displayRelic[index]);
        // Relic selected = displayRelic[index];
        //GameManager.Instance.relicInventory.AddRelic(selected); 
        foreach(var i in player.carriedRelic)
        {
            Debug.Log("relic:" + i.name + "count:" +i.triggerDescription);
        }




        displayRelic.Clear();
        gameObject.SetActive(false);
    }
}
