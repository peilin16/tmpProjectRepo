using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class LevelManager
{
    public Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

    public LevelManager()
    {
        LoadLevels();
    }

    public LevelData GetLevel(string name)
    {
        if (levels.TryGetValue(name, out LevelData level))
            return level;

        Debug.LogWarning($"Level '{name}' not found.");
        return null;
    }

    private void LoadLevels()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("levels");
        if (jsonText == null)
        {
            Debug.LogError("levels.json not found in Resources folder!");
            return;
        }

        JArray root = JArray.Parse(jsonText.text);
        foreach (var obj in root)
        {
            LevelData level = new LevelData();
            level.JsonLoad((JObject)obj);
            levels[level.name] = level;
        }
    }







}
