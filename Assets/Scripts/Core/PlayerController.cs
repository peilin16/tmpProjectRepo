using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;



public class PlayerController : MonoBehaviour ,Controller
{

    public PlayerCharacter player;

    public Character character
    {
        get => player;
        set => player = (PlayerCharacter)value;
    }
    public bool IsDead
    {
        get => dead;
        set => dead = value;
    }
    private bool dead;
    public HealthBar HealthUI
    {
        get => healthui;
        set => healthui = value;
    }
    public SpriteRenderer characterRenderer;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellUI spellui;
    //public SpellUI[] spellUIs; // Array of 2 UI panels
    public PlayerTile playerTile;

    public int spellNum = 1;
    private int currentSpellIndex = 0;
    public string playerID = "000001";

    public Unit unit;
    public GameObject restartUI;
    private Vector3 lastPosition;
    private float localStandStillTime; // 改为本地变量
    public List<Relic> carriedRelic = new List<Relic>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        unit = GetComponent<Unit>();

        GameManager.Instance.player = this.gameObject;
        EventBus.Instance.OnPlayerDeath += HandlePlayerDeath;

    }
    public void loadCharacter(int index = 0)
    {
        //Relic effect test
        //this.player.characterIndex = index;
        player = new PlayerCharacter(gameObject, index);
        //var r = GameManager.Instance.relicManager.GetRelic<GreenGem> ("Green Gem");
        //var r = GameManager.Instance.relicManager.GetRelic<CursedScroll> ("Cursed Scroll");
        //var r = GameManager.Instance.relicManager.GetRelic<JadeElephant>("Jade Elephant");
        //var r = GameManager.Instance.relicManager.GetRelic<GoldenMask>("Golden Mask");
        //var r = GameManager.Instance.relicManager.GetRelic<MysteriousMask> ("Mysterious Mask");
        //var r = GameManager.Instance.relicManager.GetRelic<KnightShield> ("Knight Shield");
        //var r = GameManager.Instance.relicManager.GetRelic<GoldenCrown> ("Golden Crown");
        var r = GameManager.Instance.relicManager.GetRelic<RedNecklace> ("Red Necklace");
        r.Application(this);
        carriedRelic.Add(r);
    }


    public void StartLevel()
    {
        //Test instance
        if(this.player == null)
            loadCharacter();


        playerTile.SetClassSprite(GameManager.Instance.playerSpriteManager.currentIconIndex);
        player.characterIndex = GameManager.Instance.playerSpriteManager.currentIconIndex;
        //reset player data
        player.StartLevel();
        // Initialize UI
        healthui.SetHealth(player.hp);
        manaui.SetSpellCaster(player.spellcaster);
        player.spellcaster.playerController = this;
        
        StartCoroutine(player.spellcaster.ManaRegeneration());
        foreach (var relic in carriedRelic)
        {
            //CoroutineManager.Instance.StartManagedCoroutine("Player_carried_relic", relic.name, relic.RelicCoroutine(this));
            relic.StartLevel(this);
        }
        //spellui.SetSpell(spellcaster.spell);
        /*spellUIs[0].SetSpell(spellcaster.spells[0]); // Arcane Bolt
        spellUIs[1].SetSpell(spellcaster.spells[1]); // Magic Missile*/
        // Start mana regen
        this.StartWave();

    }
    public void StartWave()
    {
        foreach (var relic in carriedRelic)
        {
            relic.StartWave(this);
        }
    }
    // Update is called once per frame
     void Update()
    {
        var state = GameManager.Instance.state;
        if (state == GameManager.GameState.INWAVE)
        {
            if (!GameManager.Instance.isTiming)
            {
                GameManager.Instance.isTiming = true;
                GameManager.Instance.waveSpendTime = 0f;
            }
            GameManager.Instance.waveSpendTime += Time.deltaTime;
            // presss 1 to 4 to switch the spell
            for (int i = 0; i < 4; i++)
            {
                Key key = Key.Digit1 + i;
                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    if (i < player.spellcaster.spells.Count)
                    {
                        currentSpellIndex = i;
                        Debug.Log($"Switched to spell {i + 1}: {player.spellcaster.spells[i].GetName()}");
                    }
                    else
                    {
                        Debug.Log($"No spell equipped in slot {i + 1}");
                    }
                }
            }

        }
        else if (state == GameManager.GameState.WAVEEND || state == GameManager.GameState.GAMEOVER)
        {
            if (GameManager.Instance.isTiming)
            {
                GameManager.Instance.isTiming = false;
            }
        }

        foreach (var relic in carriedRelic)
        {
            relic.Update(this); // Pass PlayerController to relic
        }
        //Stop checking
        if ((transform.position - lastPosition).sqrMagnitude < 0.001f)
        {
            localStandStillTime += Time.deltaTime;
            if (localStandStillTime >= 3f)
            {
                EventBus.Instance.TriggerStandStill();
                localStandStillTime = 0f;
            }
        }
        else
        {
            localStandStillTime = 0f;
        }
        lastPosition = transform.position;

    }
    public void TakeRelic(Relic relic)
    {
        relic.Application(this);
        carriedRelic.Add(relic);
    }
     void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(player.spellcaster.Cast(transform.position, mouseWorld, currentSpellIndex));
        //StartCoroutine(spellcaster.modifierCast(transform.position, mouseWorld, currentSpellIndex));


    }

     void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*player.speed;
    }
    private void HandlePlayerDeath(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Die(); // existing method
        }
    }
    public void Die()
    {
        player.Die();
        //Debug.Log("aaa");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        GameManager.Instance.RestartGame();
    }

}
