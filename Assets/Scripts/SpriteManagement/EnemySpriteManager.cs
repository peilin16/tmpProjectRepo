using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json; 
public class EnemySpriteManager : IconManager
{
    
    public Dictionary<string, EnemySprite> EnemyData { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.Instance.enemySpriteManager = this;
        //Debug.Log("Awake1");
        
        Initialize();
    }
    private void Initialize()
    {
        LoadEnemyData();
    }

    private void LoadEnemyData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("enemies");
        if (jsonFile == null)
        {
            Debug.LogError("can not load enemies.json ");
            return;
        }

        try
        {
            List<EnemySprite> enemies = JsonConvert.DeserializeObject<List<EnemySprite>>(jsonFile.text);
            EnemyData = new Dictionary<string, EnemySprite>();
            
            foreach (var enemy in enemies)
            {
                EnemyData.Add(enemy.name, enemy);
                // Debug.Log($"name: {enemy.name} (HP: {enemy.hp}, speed: {enemy.speed})");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"fail to read enemies.json: {e.Message}");
        }
    }


    public EnemySprite GetEnemyData(string enemyName)
    {
        if (EnemyData.TryGetValue(enemyName, out EnemySprite data))
        {
            return data;
        }
        Debug.LogWarning($"can not found enemy: {enemyName}");
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
