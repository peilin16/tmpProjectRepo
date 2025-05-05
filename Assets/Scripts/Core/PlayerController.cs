using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;
    //public SpellUI[] spellUIs; // Array of 2 UI panels
    public int speed;

    public int spellNum = 1;
    private int currentSpellIndex = 0;


    public Unit unit;
    public GameObject restartUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        //spellui.SetSpell(spellcaster.spell);
        /*spellUIs[0].SetSpell(spellcaster.spells[0]); // Arcane Bolt
        spellUIs[1].SetSpell(spellcaster.spells[1]); // Magic Missile*/

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
                    if (i < spellcaster.spells.Count)
                    {
                        currentSpellIndex = i;
                        Debug.Log($"Switched to spell {i + 1}: {spellcaster.spells[i].GetName()}");
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
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld, currentSpellIndex));
        StartCoroutine(spellcaster.modifierCast(transform.position, mouseWorld, currentSpellIndex));


    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        GameManager.Instance.DestroyAllEnemies();
    }

}
