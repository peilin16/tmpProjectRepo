using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public GameObject restartUI;
    //public TextMeshProUGUI restartUITitle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            rewardUI.SetActive(true);
            rewardUI.SetActive(!(GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves));
            restartUI.SetActive(GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves);
           // restartUITitle.text = GameManager.Instance.currentWave + 1 == GameManager.Instance.maxWaves ? "YOU WIN" : "GAME OVER";
        }
        else if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            restartUI.SetActive(true);
            rewardUI.SetActive(false);
        }
        else
        {
            rewardUI.SetActive(false);
            restartUI.SetActive(false);
        }
       /* var rewardWave = rewardUI.transform.Find("Wave").GetComponent<TextMeshProUGUI>();
        var restartWave = restartUI.transform.Find("Wave").GetComponent<TextMeshProUGUI>();
        /*rewardWave.text = $"Wave {GameManager.Instance.currentWave + 1}";
        restartWave.text = $"Wave {GameManager.Instance.currentWave + 1}";*/
    }
}
