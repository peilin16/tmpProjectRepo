using UnityEngine;
using Newtonsoft.Json.Linq;



public class Character
{
    public int speed;
    public Hittable hp;
    public int iconIndex;
    public GameObject gameObject;
    public float health;
    //level start
    public virtual void StartLevel()
    {
        this.StartWave();
    }
    
    protected virtual void JsonLoad(JObject obj)
    {
    }
    //wave start
    public virtual void StartWave()
    {

    }
}
