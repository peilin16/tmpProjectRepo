using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class EnemyCharacterManager
{
    public Dictionary<string, EnemyCharacter> enemyDB = new Dictionary<string, EnemyCharacter>();

    public EnemyCharacterManager()
    {
        LoadEnemies();
    }


    public EnemyCharacter GetEnemy(string type)
    {
        if (enemyDB.ContainsKey(type))
            return enemyDB[type];
        Debug.LogWarning($"Enemy of type {type} not found.");
        return null;
    }

    // load enemies when game start
    private void LoadEnemies()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("enemies");
        if (jsonText == null)
        {
            Debug.LogError("enemies.json not found in Resources!");
            return;
        }

        JArray root = JArray.Parse(jsonText.text);
        foreach (var obj in root)
        {
            string name = obj["name"].ToString();
            int spriteIndex = (int)obj["sprite"];
            int hp = (int)obj["hp"];
            int speed = (int)obj["speed"];
            float damage = (float)obj["damage"];

            /*if (!System.Enum.TryParse(name, true, out EnemyType type))
            {
                Debug.LogWarning($"Unknown enemy type: {name}");
                continue;
            }*/

            EnemySprite sprite = new EnemySprite(name, spriteIndex, hp, speed, damage);
            EnemyCharacter character = new EnemyCharacter(sprite, name);
            enemyDB[name] = character;
        }
    }


    public List<EnemyCharacter> GetAllEnemy()
    {
        if (enemyDB.Count == 0)
            LoadEnemies();
        return new List<EnemyCharacter>(enemyDB.Values);
    }

}