using UnityEngine;
using Newtonsoft.Json.Linq;
public class PlayerCharacter
{
    public Hittable hp;

    public int speed;
    public SpellCaster spellcaster;
    public int characterIndex = 0;
    //public float spellpower;
    public GameObject playerObject;
    public string mana_exp;
    public string hp_exp;
    public string pow_exp;
    public string speed_exp;
    public PlayerCharacter(GameObject obj)
    {
        playerObject = obj;
        LoadCharacter();

    }
    public PlayerCharacter(GameObject obj, int iconIndex = 0)
    {
        playerObject = obj;
        this.characterIndex = iconIndex;
        LoadCharacter();
    }
    
    public virtual void StartLevel()
    {
        int wave = GameManager.Instance.currentWave;
        this.spellcaster.spellPower = RPNCalculator.EvaluateFloat(pow_exp, wave);
        this.spellcaster.mana = (int)RPNCalculator.EvaluateFloat(mana_exp, wave);
        this.hp.SetMaxHP((int)RPNCalculator.EvaluateFloat(hp_exp, wave));
        this.speed = (int)RPNCalculator.EvaluateFloat(speed_exp, wave);
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

        int wave = GameManager.Instance.currentWave;
        //int power = 10;

        // Evaluate RPN expressions from JSON
        float health = RPNCalculator.EvaluateFloat(classData["health"].ToString(), wave);
        float mana = RPNCalculator.EvaluateFloat(classData["mana"].ToString(), wave);
        float manaReg = RPNCalculator.EvaluateFloat(classData["mana_regeneration"].ToString(), wave);
        float moveSpeed = RPNCalculator.EvaluateFloat(classData["speed"].ToString(), wave);
        float spellpower = RPNCalculator.EvaluateFloat(classData["spellpower"].ToString(), wave);
        speed = Mathf.RoundToInt(moveSpeed);
        this.mana_exp = classData["mana"].ToString();
        this.hp_exp = classData["health"].ToString();
        this.speed_exp = classData["speed"].ToString();
        this.pow_exp = classData["spellpower"].ToString();
        // Initialize gameplay components
        spellcaster = new SpellCaster(Mathf.RoundToInt(mana), Mathf.RoundToInt(manaReg), Hittable.Team.PLAYER, spellpower);
        hp = new Hittable(Mathf.RoundToInt(health), Hittable.Team.PLAYER, playerObject);
        //hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;
        
        Debug.Log($"{classKey} initialized with {health} HP, {mana} Mana, {manaReg}/s regen, spellpower {spellpower}, speed {speed}");
    }
    public virtual void OnAttack()
    {

    }
    public virtual void Die()
    {

    }
    public virtual void OnMove()
    {

    }


}
