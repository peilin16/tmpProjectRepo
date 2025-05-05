using UnityEngine;
using TMPro;

public class DifficultyText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = GameManager.Instance.difficultly;
    }
}
