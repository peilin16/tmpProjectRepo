using UnityEngine;
using System.Collections;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }
    //ui manager
    // Update is called once per frame
    

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
