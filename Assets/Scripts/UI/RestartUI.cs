using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RestartUI : MonoBehaviour
{
    public GameObject restartUI;
    public static RestartUI Instance;
    void Awake()
    {
        Instance = this;
        restartUI.SetActive(false);
    }

    public void Show()
    {
        restartUI.SetActive(true);
    }
    public void RestartGame()
    {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}