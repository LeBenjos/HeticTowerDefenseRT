using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemyPool enemyPool;
    private Transform target;
    [SerializeField] private float spawnRadius = 3f;

    private float timeBetweenSpawns;
    private int enemiesPerWave;
    private float spawnTimer;
    private bool isSpawning = false;

    void Start()
    {
        spawnTimer = 0f;
    }

    void Update()
    {
        if (!isSpawning)
            return;

        if (spawnTimer <= 0f)
        {
            SpawnEnemies();
            spawnTimer = timeBetweenSpawns;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector2 circlePos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + target.position;

        GameObject enemy = enemyPool.GetEnemy();
        enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().Initialize(target, enemyPool);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void UpdateSpawnSettings(SpawnSettings newSettings)
    {
        timeBetweenSpawns = newSettings.TimeBetweenSpawns;
        enemiesPerWave = newSettings.EnemiesPerWave;
    }

    public void StartContinuousSpawn()
    {
        isSpawning = true;
    }

    public void StopContinuousSpawn()
    {
        isSpawning = false;
    }
}
