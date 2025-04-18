using System.Collections.Generic;
using UnityEngine;


public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyPrefabEntry
    {
        public EnemyType type;
        public GameObject prefab;
    }

    [SerializeField] private List<EnemyPrefabEntry> enemyPrefabs;
    [SerializeField] private int initialPoolSize = 10;

    private readonly Dictionary<EnemyType, Queue<GameObject>> enemyPools = new();
    private readonly Dictionary<EnemyType, GameObject> prefabLookup = new();

    void Awake()
    {
        foreach (var entry in enemyPrefabs)
        {
            prefabLookup[entry.type] = entry.prefab;
            enemyPools[entry.type] = new Queue<GameObject>();

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject enemy = Instantiate(entry.prefab);
                enemy.SetActive(false);
                enemyPools[entry.type].Enqueue(enemy);
            }
        }
    }

    public GameObject GetEnemy(EnemyType type)
    {
        if (!enemyPools.ContainsKey(type))
        {
            Debug.LogWarning($"No pool found for enemy type {type}");
            return null;
        }

        if (enemyPools[type].Count == 0)
        {
            GameObject newEnemy = Instantiate(prefabLookup[type]);
            return newEnemy;
        }

        GameObject enemy = enemyPools[type].Dequeue();
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            EnemyType type = enemyBase.EnemyType;
            enemyPools[type].Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning("Returned object doesn't have EnemyBase!");
        }
    }
}
