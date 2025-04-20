using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float bossSpawnChance = 0.01f;
    [SerializeField] private float runnerSpawnChance = 0.15f;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private int enemiesPerWave = 3;
    private Transform target;
    private float spawnTimer;
    private bool isSpawning = false;

    void Start()
    {
        spawnTimer = 0f;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += StopContinuousSpawn;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= StopContinuousSpawn;
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

        EnemyType typeToSpawn = GetRandomEnemyType();

        GameObject enemy = enemyPool.GetEnemy(typeToSpawn);
        if (enemy == null) return;

        enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        enemy.SetActive(true);

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.Initialize(target, enemyPool);
    }

    private EnemyType GetRandomEnemyType()
    {
        float spawnRoll = Random.value;

        if (spawnRoll < bossSpawnChance)
        {
            return EnemyType.Boss;
        }
        else if (spawnRoll < bossSpawnChance + runnerSpawnChance)
        {
            return EnemyType.Runner;
        }
        else
        {
            return EnemyType.Minion;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void UpdateSpawnSettings(float newTimeBetweenSpawns, int newEnemiesPerWave)
    {
        timeBetweenSpawns = newTimeBetweenSpawns;
        enemiesPerWave = newEnemiesPerWave;
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
