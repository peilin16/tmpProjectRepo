using UnityEngine;
using Newtonsoft.Json.Linq;
public class PlayerCharacter : Character
{

    public SpellCaster spellcaster;
    public int characterIndex = 0;
    public float spellpower;

    public string mana_exp;
    public string hp_exp;
    public string pow_exp;
    public string speed_exp;

    public int manaReg;
    public int mana;
    public PlayerCharacter(GameObject obj)
    {
        gameObject = obj;
        LoadCharacter();

    }
    public PlayerCharacter(GameObject obj, int iconIndex = 0)
    {
        gameObject = obj;
        this.characterIndex = iconIndex;
        LoadCharacter();
    }
    
    public override void StartLevel()
    {
        int wave = GameManager.Instance.currentWave;
        this.spellcaster.spellPower = RPNCalculator.EvaluateFloat(pow_exp, wave);
        this.spellcaster.mana = (int)RPNCalculator.EvaluateFloat(mana_exp, wave);
        this.hp.SetMaxHP((int)RPNCalculator.EvaluateFloat(hp_exp, wave));
        base.speed = (int)RPNCalculator.EvaluateFloat(speed_exp, wave);
        this.hp.hp = this.hp.max_hp;
    }
    protected void LoadCharacter()
    {
        string[] characterKeys = { "mage", "warlock", "battlemage" };

        if (characterIndex < 0 || characterIndex >= characterKeys.Length)
        {
            Debug.LogError($"Invalid characterIndex: {characterIndex}");
            return;
        }

        // Load class data from Resources
        TextAsset jsonText = Resources.Load<TextAsset>("classes");
        if (jsonText == null)
        {
            Debug.LogError("classes.json not found in Resources!");
            return;
        }

        JObject root = JObject.Parse(jsonText.text);
        string classKey = characterKeys[characterIndex];
        JObject classData = (JObject)root[classKey];

        this.JsonLoad(classData);


        Debug.Log($"{classKey} initialized with {base.health} HP, {mana} Mana, {manaReg}/s regen, spellpower {spellpower}, speed {base.speed}");
    }


    protected override void JsonLoad(JObject obj)
    {
        int wave = GameManager.Instance.currentWave;
        //int power = 10;

        // Evaluate RPN expressions from JSON
        float health = RPNCalculator.EvaluateFloat(obj["health"].ToString(), wave);
        float mana = RPNCalculator.EvaluateFloat(obj["mana"].ToString(), wave);
        float manaReg = RPNCalculator.EvaluateFloat(obj["mana_regeneration"].ToString(), wave);
        float moveSpeed = RPNCalculator.EvaluateFloat(obj["speed"].ToString(), wave);
        float spellpower = RPNCalculator.EvaluateFloat(obj["spellpower"].ToString(), wave);
        base.speed = Mathf.RoundToInt(moveSpeed);
        this.mana_exp = obj["mana"].ToString();
        this.hp_exp = obj["health"].ToString();
        this.speed_exp = obj["speed"].ToString();
        this.pow_exp = obj["spellpower"].ToString();
        base.health = health;
        // Initialize gameplay components
        spellcaster = new SpellCaster(Mathf.RoundToInt(mana), Mathf.RoundToInt(manaReg), Hittable.Team.PLAYER, spellpower);
        hp = new Hittable(Mathf.RoundToInt(health), Hittable.Team.PLAYER, gameObject);
        //hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;
    }

    public void OnAttack()
    {

    }
    public void Die()
    {

    }
    public void OnMove()
    {

    }


}
