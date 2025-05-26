using UnityEngine;
using Newtonsoft.Json.Linq;





public class EnemyCharacter : Character
{
    public EnemySprite enemySprite;
    private int _final_healthly;
    public int final_healthly
    {
        get => this._final_healthly;
        set
        {
            this._final_healthly = value;  
            if (this.hp == null && this.gameObject != null)
                this.hp = new Hittable(this._final_healthly, Hittable.Team.MONSTERS, gameObject);
            else if(this.hp != null)
                this.hp.SetMaxHP(_final_healthly);
        }
    }
    public float final_speed;
    public float final_damage;

    public string type;

    public EnemyCharacter() { }

    public EnemyCharacter(EnemySprite sprite, string type)
    {
        this.enemySprite = sprite;
        this.type = type;
        this._final_healthly = sprite.healthly;
        this.final_speed = sprite.speed;
        this.final_damage = sprite.damage;
        
    }
    
    public override void StartLevel() {
        this.StartWave();
    }
    public override void StartWave() {
        if (this.hp == null)
            this.hp = new Hittable(this._final_healthly, Hittable.Team.MONSTERS, gameObject);

    }
    protected override void JsonLoad(JObject obj)
    {
        //not use
    }

    public void UpdateLevel(int wave)
    {
        // Placeholder for logic that modifies stats based on wave
    }
}
