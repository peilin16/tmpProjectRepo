using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class LevelData
{
    public string name;
    public int total_waves;
    public int current_wave = 1;
    public int delay_sec = 2;

    public List<LevelSpawn> Spawns = new List<LevelSpawn>();
    public void JsonLoad(JObject obj)
    {
        name = obj["name"]?.ToString();
        Debug.Log("data:" + name);
        total_waves = (int)(obj["waves"] ?? 1);

        foreach (JObject spawn in obj["spawns"])
        {
            LevelSpawn levelSpawn = new LevelSpawn();
            levelSpawn.JsonLoad(spawn);
      
            Spawns.Add(levelSpawn);
        }


    }
    public void StartLevel() { }
    public void StartWave()
    {
        //current_wave = 1;
        //return null; // integrate later if needed
    }
    private void UpdateLevel(int wave) {
        foreach (var spawn in Spawns)
        {
            spawn.UpdateLevel(wave);
        }
    }
    public void NextWave()
    {
        this.current_wave = GameManager.Instance.currentWave;
        UpdateLevel(current_wave);
    }
    public void SpawnEnemy(int index) {
    }
    public void StartSpawn() { }

    private void NextSpawn() { }



}