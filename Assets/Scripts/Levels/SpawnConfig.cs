using UnityEngine;
using System;
using Newtonsoft.Json.Linq;




[Serializable]
public class SpawnConfig
{
    public string enemy;
    public string count;
    public string hp;
    public string speed;
    public string damage;
    public string delay;
    public int[] sequence;
    public string location;
    
    public SpawnConfig() { }
    
    public SpawnConfig(
        string enemy,
        string count,
        string hp,
        string speed = null,
        string damage = null,
        string delay = null,
        int[] sequence = null,
        string location = null)
    {
        this.enemy = enemy;
        this.count = count;
        this.hp = hp;
        this.speed = speed;
        this.damage = damage;
        this.delay = delay;
        this.sequence = sequence ?? new int[] { 1 }; 
        this.location = location ?? "random"; 
    }
    
    public static SpawnConfig FromJObject(JObject spawnObj)
    {
        if (spawnObj == null) return null;
        
        int[] sequence = null;
        if (spawnObj["sequence"] is JArray sequenceArray)
        {
            sequence = new int[sequenceArray.Count];
            for (int i = 0; i < sequenceArray.Count; i++)
            {
                sequence[i] = (int)sequenceArray[i];
            }
        }

        return new SpawnConfig(
            enemy: spawnObj["enemy"]?.ToString(),
            count: spawnObj["count"]?.ToString(),
            hp: spawnObj["hp"]?.ToString(),
            speed: spawnObj["speed"]?.ToString(),
            damage: spawnObj["damage"]?.ToString(),
            delay: spawnObj["delay"]?.ToString(),
            sequence: sequence,
            location: spawnObj["location"]?.ToString()
        );
    }
    
    public override string ToString()
    {
        return $"[SpawnConfig: {enemy}] " +
               $"Count={count}, " +
               $"HP={hp}, " +
               $"Delay={delay}, " +
               $"Sequence={string.Join(",", sequence)}, " +
               $"Location={location}";
    }
}