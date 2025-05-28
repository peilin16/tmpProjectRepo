using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;

    private LevelData currentLevel;

    void Start()
    {
        CreateButton("Easy", 40);
        CreateButton("Medium", 0);
        CreateButton("Endless", -40);
    }

    void CreateButton(string label, float yOffset)
    {
        GameObject btn = Instantiate(button, level_selector.transform);
        btn.transform.localPosition = new Vector3(0, yOffset);
        btn.GetComponent<MenuSelectorController>().spawner = this;
        btn.GetComponent<MenuSelectorController>().SetLevel(label);
    }

    public void StartLevel(string levelname)
    {
        GameManager.Instance.difficultly = levelname;


        if (levelname.Equals("Medium"))
        {

            GameManager.Instance.level = GameManager.Difficultly.Medium;
        }
        else if((levelname.Equals("Endless")))
        {
            GameManager.Instance.level = GameManager.Difficultly.Endless;
        }
        GameManager.Instance.currentWave = 1;
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        Debug.Log(levelname);
        currentLevel = GameManager.Instance.levelManager.GetLevel(levelname);
        level_selector.gameObject.SetActive(false);

        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        CoroutineManager.Instance.StartManagedCoroutine("EnemySpawn","wave "+ GameManager.Instance.currentWave, SpawnWave());
        //StartCoroutine(SpawnWave());
    }

    public void NextWave()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            GameManager.Instance.currentWave += 1;
            currentLevel.NextWave();
            GameManager.Instance.player.GetComponent<PlayerController>().StartWave();
            if (GameManager.Instance.currentWave <= currentLevel.total_waves)
                CoroutineManager.Instance.StartManagedCoroutine("EnemySpawn", "wave " + GameManager.Instance.currentWave, SpawnWave());
            //StartCoroutine(SpawnWave());
            else
                Debug.Log("All waves complete.");
        }
    }

    IEnumerator SpawnWave()
    {

        GameManager.Instance.state = GameManager.GameState.INWAVE;
        //Debug.Log("Next Wave");
        //currentLevel.NextWave();
        foreach (var spawn in currentLevel.Spawns)
        {
            int i = 0;
            int spawned = 0;
            int total = spawn.final_count;
            List<int> sequence = spawn.sequence ?? new List<int> { 1 };
            while (spawned < total)
            {
                int groupSize = sequence[i % sequence.Count];
                for (int j = 0; j < groupSize && spawned < total; j++)
                {
                    
                    SpawnEnemy(spawn.enemySequence[spawned], spawn.location);
                    spawned++;
                }
                Debug.Log("Success spawn");
                yield return new WaitForSeconds(currentLevel.delay_sec);
                i++;
            }
        }
        
        yield return new WaitWhile(() => GameManager.Instance.enemyManager.enemy_count > 0);

        if (GameManager.Instance.state != GameManager.GameState.GAMEOVER)
            GameManager.Instance.state = GameManager.GameState.WAVEEND;
    }
    //spawn enemy
    void SpawnEnemy(EnemyCharacter character, string location)
    {
        Vector3 pos = PickSpawnPoint(location);
        GameObject enemyObj = Instantiate(enemy, pos, Quaternion.identity);
        
        enemyObj.GetComponent<SpriteRenderer>().sprite =
            GameManager.Instance.enemySpriteManager.Get(character.enemySprite.spriteIndex);
        
        EnemyController controller = enemyObj.GetComponent<EnemyController>();
        controller.character = character; // assign the character first
        controller.character.gameObject = enemyObj; //set gameObject
        controller.character.StartWave();
        GameManager.Instance.enemyManager.AddEnemy(enemyObj);
    }

    Vector3 PickSpawnPoint(string location)
    {
        List<SpawnPoint> filtered = SpawnPoints
            .Where(p => location == "random" || p.tag.ToLower().Contains(location.Replace("random", "").Trim().ToLower()))
            .ToList();

        if (filtered.Count == 0) filtered = SpawnPoints.ToList();

        Vector3 basePos = filtered[UnityEngine.Random.Range(0, filtered.Count)].transform.position;
        Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.5f;
        return basePos + new Vector3(offset.x, offset.y, 0);
    }
}
