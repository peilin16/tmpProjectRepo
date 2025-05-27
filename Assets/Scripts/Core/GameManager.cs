using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;
    public enum Difficultly {Easy, Medium, Endless}
    public Difficultly level = Difficultly.Easy;
    public int currentWave = 1;
    public int maxWaves = 0;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
            {
                theInstance = new GameManager();
                theInstance.relicManager = new RelicManager();
                theInstance.enemyCharacterManager = new EnemyCharacterManager();
                theInstance.levelManager = new LevelManager();
                theInstance.enemyManager = new EnemyManager();
            }    
            return theInstance;
        }
    }

    public GameObject player;
    public LevelManager levelManager;
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public EnemyCharacterManager enemyCharacterManager;
    public EnemyManager enemyManager;

    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;
    public RelicManager relicManager;


    public int defectCount;
    public float waveSpendTime = 0f;
    public bool isTiming = false;
    public string difficultly = "Easy";

    public void RestartGame()
    {
        enemyManager.DestroyAllEnemies();
        CoroutineManager.Instance.StopGroup("EnemySpawn");
        //player.player.hp.hp = 1;
    }
}
