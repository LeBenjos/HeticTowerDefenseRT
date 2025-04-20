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
                GameObject enemy = Instantiate(entry.prefab, transform);
                enemy.SetActive(false);
                enemyPools[entry.type].Enqueue(enemy);
            }
        }
    }

    public GameObject Get(EnemyType type)
    {
        if (!enemyPools.ContainsKey(type))
        {
            Debug.LogWarning($"No pool found for enemy type {type}");
            return null;
        }

        GameObject enemy;
        if (enemyPools[type].Count == 0)
        {
            enemy = Instantiate(prefabLookup[type], transform);
        }
        else
        {
            enemy = enemyPools[type].Dequeue();
        }

        enemy.SetActive(true);
        return enemy;
    }

    public void Return(GameObject enemy)
    {
        enemy.SetActive(false);

        if (enemy.TryGetComponent<EnemyBase>(out var enemyBase))
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
