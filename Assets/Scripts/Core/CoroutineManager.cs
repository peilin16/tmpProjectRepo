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

    // 存储协程：group -> id -> Coroutine
    private Dictionary<string, Dictionary<string, Coroutine>> groupedCoroutines = new Dictionary<string, Dictionary<string, Coroutine>>();

    /// <summary>
    /// 启动协程（带 ID 和分组），自动覆盖同 ID
    /// </summary>
    public void StartManagedCoroutine(string group, string id, IEnumerator routine)
    {
        if (!groupedCoroutines.ContainsKey(group))
            groupedCoroutines[group] = new Dictionary<string, Coroutine>();

        // 停止旧协程（如果已存在）
        if (groupedCoroutines[group].TryGetValue(id, out var existing))
        {
            StopCoroutine(existing);
        }

        Coroutine c = StartCoroutine(WrappedCoroutine(group, id, routine));
        groupedCoroutines[group][id] = c;
    }

    /// <summary>
    /// 包装协程，在结束时自动移除记录
    /// </summary>
    private IEnumerator WrappedCoroutine(string group, string id, IEnumerator routine)
    {
        yield return routine;
        groupedCoroutines[group].Remove(id);
        if (groupedCoroutines[group].Count == 0)
            groupedCoroutines.Remove(group);
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
