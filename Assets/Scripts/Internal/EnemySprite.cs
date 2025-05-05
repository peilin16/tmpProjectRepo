using UnityEngine;

[System.Serializable]
public class EnemySprite
{
    public string name;
    public int spriteIndex;
    public int hp;
    public int speed;
    public float damage;

    public EnemySprite(string name, int spriteIndex, int hp, int speed, float damage)
    {
        this.name = name;
        this.spriteIndex = spriteIndex;
        this.hp = hp;
        this.speed = speed;
        this.damage = damage;
    }
}