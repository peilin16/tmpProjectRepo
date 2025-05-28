using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;



public class RelicManager
{
    private Dictionary<string, Relic> relics = new Dictionary<string, Relic>();
    public void LoadRelic()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("relics");
        if (jsonText == null)
        {
            Debug.LogError("relics.json not found in Resources!");
            return;
        }

        JArray root = JArray.Parse(jsonText.text);

        foreach (JObject obj in root)
        {
            string name = obj["name"].ToString();

            Relic relic = null;
            switch (name)
            {
                case "Green Gem":
                    relic = new GreenGem();
                    break;
                case "Jade Elephant":
                    relic = new JadeElephant();
                    break;
                case "Golden Mask":
                    relic = new GoldenMask();
                    break;
                case "Cursed Scroll":
                    relic = new CursedScroll();
                    break;
                case "Mysterious Mask":
                    relic = new MysteriousMask();
                    break;
                case "Knight Shield":
                    relic = new KnightShield();
                    break;
                case "Golden Crown":
                    relic = new GoldenCrown();
                    break;
                case "Grand Chronicle":
                    relic = new GrandChronicle();
                    break;
                case "Red Necklace":
                    relic = new RedNecklace();
                    break;
                default:
                    Debug.LogWarning($"Unknown relic: {name}");
                    continue;
            }

            relic.JsonInit(obj); // Initialize all data here
            relics[relic.name] = relic;
        }

        Debug.Log($"Loaded {relics.Count} relics.");
    }

    public List<Relic> GetAllRelics()
    {
        if (relics.Count == 0)
            LoadRelic();
        return new List<Relic>(relics.Values);
    }

    public T GetRelic<T>(string name) where T : Relic
    {
        if (relics.Count == 0)
            LoadRelic();

        if (relics.TryGetValue(name, out var relic) && relic is T typedRelic)
            return typedRelic;

        Debug.LogWarning($"Relic '{name}' of type {typeof(T).Name} not found.");
        return null;
    }
}
