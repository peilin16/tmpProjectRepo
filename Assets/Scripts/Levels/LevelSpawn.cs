using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class LevelSpawn
{

 
    public int final_count;

    public string count_exp;
    public string hp_exp;
    public string damage_exp;
    public string location;
    public string enemytype;
    public List<EnemyCharacter> enemySequence = new List<EnemyCharacter>();
    public List<int> sequence;

    public void JsonLoad(JObject obj)
    {
        //Debug.Log("type:");
        //int power = 10;
        int wave = GameManager.Instance.currentWave;
        // Parse enemy type
        enemytype = obj["enemy"]?.ToString();
        count_exp = obj["count"]?.ToString();
        hp_exp = obj["hp"]?.ToString();
        damage_exp = obj["damage"]?.ToString();
        location = obj["location"]?.ToString();

        var seq = obj["sequence"];
        if (seq != null)
            sequence = seq.ToObject<List<int>>();
        //get enemy count
        //Debug.Log("type:");
        final_count = Mathf.FloorToInt(RPNCalculator.EvaluateFloat(count_exp, wave));
        //Debug.Log("type:"+ enemytype + " count:" + final_count + " hp_exp:" + hp_exp +" dam_exp:"+ damage_exp);
        
        EnemyCharacter template = GameManager.Instance.enemyCharacterManager.GetEnemy(enemytype);
        // setting the level enemy data
        //Debug.Log("name: " + template.type + " healthly:" + template.final_healthly + " damage:" + template.final_damage);

        for (int i = 0; i < this.final_count; i++)
        {
            
            EnemyCharacter clone = new EnemyCharacter(template.enemySprite, enemytype);

            clone.final_healthly =  (int)RPNCalculator.CalculateBaseCount(hp_exp, wave, template.enemySprite.healthly);
            
            if (!string.IsNullOrEmpty(damage_exp))
            {

                clone.final_damage = RPNCalculator.EvaluateFloat(damage_exp, wave);
            }
            else
                clone.final_damage = template.final_damage;

            clone.final_speed = template.final_speed;
           // Debug.Log("name: " + clone.type + " healthly:"+ clone.final_healthly + " damage:" + clone.final_damage);
            enemySequence.Add(clone);
        }
    }
    public void UpdateLevel(int wave)
    {
        enemySequence.Clear();

        EnemyCharacterManager enemyCharacterManager = GameManager.Instance.enemyCharacterManager;
        EnemyCharacter baseEnemy = enemyCharacterManager.GetEnemy(enemytype);

        // Calculate final count


        final_count = Mathf.FloorToInt(RPNCalculator.EvaluateFloat(count_exp ,wave));

        for (int i = 0; i < final_count; i++)
        {
            EnemyCharacter clone = new EnemyCharacter(baseEnemy.enemySprite, enemytype);
            //Debug.Log("heal" + hp_exp);
            clone.final_healthly = Mathf.FloorToInt(RPNCalculator.CalculateBaseCount(hp_exp , wave, baseEnemy.enemySprite.healthly));
            if (!string.IsNullOrEmpty(damage_exp))
            {
               // Debug.Log("dam" + damage_exp);
                clone.final_damage = RPNCalculator.EvaluateFloat(damage_exp, wave);
            }
            else
                clone.final_damage = baseEnemy.final_damage;
            clone.final_speed = baseEnemy.final_speed;
            //variables["base"] = baseEnemy.enemySprite.speed;
            // clone.final_speed = Mathf.FloorToInt(RPNCalculator.EvaluateFloat("base", variables));  // Assuming no speed_exp ¡ª default is base
            Debug.Log("name: " + clone.type + " healthly:" + clone.final_healthly + " damage:" + clone.final_damage);
            enemySequence.Add(clone);
        }
    }


}
