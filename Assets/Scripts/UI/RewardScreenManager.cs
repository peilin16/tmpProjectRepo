using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public GameObject restartUI;
    public GameObject relicUIobj;
    public RelicUI relicUI;
    public bool isDisplay = false;
    //public TextMeshProUGUI restartUITitle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND && !isDisplay)
        {
            rewardUI.SetActive(true);
            rewardUI.SetActive(!(GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves));
            restartUI.SetActive(GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves);
            isDisplay = true;
           // restartUITitle.text = GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves ? "YOU WIN" : "GAME OVER";
        }
        else if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            Debug.Log("restart");
            restartUI.SetActive(true);
            rewardUI.SetActive(false);

        }
        else if(GameManager.Instance.state != GameManager.GameState.WAVEEND)
        {
            //rewardUI.SetActive(false);
            //restartUI.SetActive(false);
            isDisplay = false;
        }
       /* var rewardWave = rewardUI.transform.Find("Wave").GetComponent<TextMeshProUGUI>();
        var restartWave = restartUI.transform.Find("Wave").GetComponent<TextMeshProUGUI>();
        /*rewardWave.text = $"Wave {GameManager.Instance.currentWave + 1}";
        restartWave.text = $"Wave {GameManager.Instance.currentWave + 1}";*/
    }

}
