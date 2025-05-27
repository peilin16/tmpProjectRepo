using UnityEngine;

public class EnemyController : MonoBehaviour, Controller
{

    public Transform target;

    public HealthBar healthui;
    public bool dead;
    //public Hittable hp;
    public float speed;

    public EnemyCharacter characterData;  // Implements Controller.character
    public float last_attack;

    public Character character
    {
        get => characterData;
        set => characterData = (EnemyCharacter)value;
    }

    public HealthBar HealthUI
    {
        get => healthui;
        set => healthui = value;
    }

    public bool IsDead
    {
        get => dead;
        set => dead = value;
    }

    //public EnemyCharacter enemy;

        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterData.gameObject = this.gameObject;
        target = GameManager.Instance.player.transform;
        //hp.OnDeath += Die;
        EventBus.Instance.OnMonsterDeath += (gameObject) => this.Die();
        healthui.SetHealth(characterData.hp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * characterData.final_speed;
        }
    }
    void StartLevel()
    {
        characterData.StartLevel();
    }
    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            target.gameObject.GetComponent<PlayerController>().player.hp.Damage(new Damage(5, Damage.Type.PHYSICAL));
        }
    }


    public void Die()
    {
        if (!dead && this.characterData.hp.hp <= 0)
        {
            dead = true;
            GameManager.Instance.enemyManager.RemoveEnemy(this.gameObject);
            Destroy(this.gameObject);
            Destroy(this.characterData.gameObject);
        }
    }
}
