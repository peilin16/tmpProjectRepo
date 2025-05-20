using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CoroutineManager : MonoBehaviour
{
    // public static CoroutineManager Instance;
    private static CoroutineManager _instance;
    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("CoroutineManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CoroutineManager>();
            }
            return _instance;
        }
    }

    // ：group -> id -> Coroutine
    private Dictionary<string, Dictionary<string, Coroutine>> groupedCoroutines = new Dictionary<string, Dictionary<string, Coroutine>>();


    public Coroutine StartManagedCoroutine(string group, string id, IEnumerator routine)
    {

        if (!groupedCoroutines.ContainsKey(group))
        {
            groupedCoroutines[group] = new Dictionary<string, Coroutine>();
        }


        if (groupedCoroutines[group].TryGetValue(id, out var existing))
        {
            StopCoroutine(existing);
            groupedCoroutines[group].Remove(id);
        }


        Coroutine c = StartCoroutine(WrappedCoroutine(group, id, routine));
        groupedCoroutines[group][id] = c; 
        return c;
    }

    /// <summary>
    /// wrapped coroutine
    /// </summary>
    private IEnumerator WrappedCoroutine(string group, string id, IEnumerator routine)
    {
        yield return routine;

        if (groupedCoroutines.TryGetValue(group, out var dict))
        {
            dict.Remove(id);
            if (dict.Count == 0)
                groupedCoroutines.Remove(group);
        }
    }

    /// <summary>
    /// 停止某个具体协程
    /// </summary>
    public void StopManagedCoroutine(string group, string id)
    {
        if (groupedCoroutines.TryGetValue(group, out var dict))
        {
            if (dict.TryGetValue(id, out var c))
            {
                StopCoroutine(c);
                dict.Remove(id);
            }
            if (dict.Count == 0)
                groupedCoroutines.Remove(group);
        }
    }

    /// <summary>
    /// 停止某个分组内所有协程
    /// </summary>
    public void StopGroup(string group)
    {
        if (groupedCoroutines.TryGetValue(group, out var dict))
        {
            foreach (var c in dict.Values)
            {
                StopCoroutine(c);
            }
            groupedCoroutines.Remove(group);
        }
    }

    /// <summary>
    /// 停止全部协程
    /// </summary>
    public void StopAllManagedCoroutines()
    {
        foreach (var group in groupedCoroutines.Values)
        {
            foreach (var c in group.Values)
            {
                StopCoroutine(c);
            }
        }
        groupedCoroutines.Clear();
    }
}
