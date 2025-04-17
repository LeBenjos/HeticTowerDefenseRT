using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 20;

    private readonly Queue<GameObject> pool = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if (pool.Count == 0)
        {
            // Optionnel : agrandir le pool
            GameObject extra = Instantiate(enemyPrefab);
            extra.SetActive(false);
            return extra;
        }

        // Debug.Log("Get enemy from pool");
        return pool.Dequeue();
    }

    public void ReturnEnemy(GameObject enemy)
    {
        // Debug.Log("Return enemy to pool");
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
