using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

public class EnemyManager
{
    public List<GameObject> enemies = new List<GameObject>();

    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        GameManager.Instance.defectCount++;
        enemies.Remove(enemy);
    }

    public void DestroyAllEnemies()
    {
        List<GameObject> enemiesToDestroy = new List<GameObject>(enemies);
        foreach (GameObject enemy in enemiesToDestroy)
        {
            if (enemy != null)
            {
                EnemyController controller = enemy.GetComponent<EnemyController>();
                if (controller != null)
                {
                    controller.Die();
                    GameObject.Destroy(enemy);
                }
            }
        }
        enemies.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a, b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }
}
