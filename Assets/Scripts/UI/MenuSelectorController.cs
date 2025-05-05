using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public string level;
    public EnemySpawner spawner;
    
    private static MenuSelectorController theInstance;
    public static MenuSelectorController Instance {  get
        {
            if (theInstance == null)
                theInstance = new MenuSelectorController();
            return theInstance;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // this.SetLevel("Start");
        // GameManager.Instance.level = GameManager.Difficultly.Easy;
        // label.text = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(string text)
    {
        this.label.text = text;
        this.level = text;
        Debug.Log(this.level + "  " + level + "  " + "IDK2");
    }

    public void StartLevel()
    {
        Debug.Log(this.level + "  " + this.label.text + "  " + "IDK");
        spawner.StartLevel(this.label.text);
    }
}
