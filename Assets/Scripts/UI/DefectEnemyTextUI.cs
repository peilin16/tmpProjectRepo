using UnityEngine;
using TMPro;

public class DefectEnemyInformati : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public int defeatEnemy = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {

        tmp.text = "Defeated: " + defeatEnemy;
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND || GameManager.Instance.state == GameManager.GameState.PREGAME)
        {
            tmp.enabled = true;
            tmp.text = "Defeated: " + GameManager.Instance.defectCount;
        }
        else
        {
            tmp.enabled = false;
        }

    }
    

}
