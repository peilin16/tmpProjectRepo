using UnityEngine;
using TMPro;

public class TimeInformation : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    void Start()
    {
        tmp.text = "Spend Time: 0s";
    }

    void Update()
    {
        tmp.text = "Spend Time: " + Mathf.Floor(GameManager.Instance.waveSpendTime).ToString("F0") + "s";
    }
}
