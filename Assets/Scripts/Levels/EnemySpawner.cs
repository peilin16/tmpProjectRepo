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
    private LevelData easyLevelData; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject easyButton = Instantiate(button, level_selector.transform);
        easyButton.transform.localPosition = new Vector3(0, 40);
        easyButton.GetComponent<MenuSelectorController>().spawner = this;
        easyButton.GetComponent<MenuSelectorController>().SetLevel("Easy");
        GameObject mediumButton = Instantiate(button, level_selector.transform);
        mediumButton.transform.localPosition = new Vector3(0, 0);
        mediumButton.GetComponent<MenuSelectorController>().spawner = this;
        mediumButton.GetComponent<MenuSelectorController>().SetLevel("Medium");
        GameObject endlessButton = Instantiate(button, level_selector.transform);
        endlessButton.transform.localPosition = new Vector3(0, -40);
        endlessButton.GetComponent<MenuSelectorController>().spawner = this;
        endlessButton.GetComponent<MenuSelectorController>().SetLevel("Endless");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadLevelData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("levels");
        if (jsonFile == null)
        {
            Debug.LogError("can not load levels.json ");
            return;
        }

        try
        {
            JArray levelsArray = JArray.Parse(jsonFile.text);
            foreach (JObject levelObj in levelsArray) {
                if (levelObj["name"]?.ToString() == GameManager.Instance.difficultly) {
                    List<SpawnConfig> spawnsList = new List<SpawnConfig>();
                    JArray spawnsArray = (JArray)levelObj["spawns"];

                    if (spawnsArray != null) {
                        foreach (JObject spawnObj in spawnsArray) {
                            SpawnConfig config = new SpawnConfig {
                                enemy = spawnObj["enemy"]?.ToString(),
                                count = spawnObj["count"]?.ToString(),
                                hp = spawnObj["hp"]?.ToString(),
                                speed = spawnObj["speed"]?.ToString(),
                                damage = spawnObj["damage"]?.ToString(),
                                delay = spawnObj["delay"]?.ToString(),
                                location = spawnObj["location"]?.ToString()
                            };

                            JArray sequenceArray = (JArray)spawnObj["sequence"];
                            if (sequenceArray != null) {
                                config.sequence = new int[sequenceArray.Count];
                                for (int i = 0; i < sequenceArray.Count; i++) {
                                    config.sequence[i] = (int)sequenceArray[i];
                                }
                            }
                            spawnsList.Add(config);
                        }
                    }
                    
                    easyLevelData = new LevelData{
                        name = levelObj["name"]?.ToString(),
                        waves = levelObj["waves"]?.ToObject<int>() ?? 0,
                        spawns = spawnsList
                    };
                    GameManager.Instance.maxWaves = levelObj["waves"]?.ToObject<int>() ?? 0;
                    // Debug.Log($"successful load {easyLevelData.spawns.Count} enemy");
                    return;
                }
            }
            
        } catch (System.Exception e) {
            Debug.LogError($"fail to load JSON: {e.Message}\n{e.StackTrace}");
        }
    }

    public void StartLevel(string levelname)
    {
        GameManager.Instance.level = levelname == "Easy" ? GameManager.Difficultly.Easy : levelname == "Medium" ? GameManager.Difficultly.Medium : GameManager.Difficultly.Endless;
        GameManager.Instance.difficultly = levelname;
        LoadLevelData();
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        GameManager.Instance.currentWave = 0;
        StartCoroutine(SpawnWave(GameManager.Instance.currentWave));
    }

    public void NextWave()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND) {
            GameManager.Instance.currentWave++;
            if (GameManager.Instance.currentWave < easyLevelData.waves) {
                StartCoroutine(SpawnWave(GameManager.Instance.currentWave));
            } else {
                Debug.Log("All waves complete!");
            }
        }
    }


    IEnumerator SpawnWave(int waveIndex)
    {
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        GameManager.Instance.countdown = 3;
        foreach (var config in easyLevelData.spawns) {
            if (config.sequence == null || waveIndex >= config.sequence.Length) continue;
            int waveNum = config.sequence[waveIndex];
            int count = RPNCalculator.CalculateEnemyCount(config.count, waveNum);
            count = Mathf.Max(1, count);
            for (int i = 0; i < count; i++) {
                SpawnEnemy(config, waveNum, config.enemy == "zombie" ? 0 : config.enemy == "skeleton" ? 1 : 2);
                yield return new WaitForSeconds(0.1f);
            }
        }
        //Debug.Log(GameManager.Instance.enemy_count);
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        //Debug.Log("Wave end");
        if (GameManager.Instance.state != GameManager.GameState.GAMEOVER) {
            Debug.Log("Wave end");
            GameManager.Instance.state = GameManager.GameState.WAVEEND;


        }
    }

    void SpawnEnemy(SpawnConfig config, int wave, int spriteIndex)
    {
        EnemySprite enemyData = GameManager.Instance.enemySpriteManager.GetEnemyData(config.enemy);
        SpawnPoint spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
        Vector3 position = spawnPoint.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject enemyObj = Instantiate(enemy, position, Quaternion.identity);
        enemyObj.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(spriteIndex);
        var variables = new Dictionary<string, float> {
            { "base", 20 },
            { "wave", wave }
        };
        enemyData.hp = RPNCalculator.CalculateEnemyCount(config.hp, wave,enemyData.hp);
        EnemyController controller = enemyObj.GetComponent<EnemyController>();
        controller.hp = new Hittable(enemyData.hp, Hittable.Team.MONSTERS, enemy);
        controller.speed = enemyData.speed;
        GameManager.Instance.AddEnemy(enemyObj);
    }
}
