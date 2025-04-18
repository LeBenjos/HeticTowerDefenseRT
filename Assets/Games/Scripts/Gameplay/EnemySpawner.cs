using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float bossSpawnChance = 0.01f;

    private Transform target;
    private float timeBetweenSpawns;
    private int enemiesPerWave;
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

        // Choisir le type à spawn
        EnemyType typeToSpawn = Random.value < bossSpawnChance ? EnemyType.Boss : EnemyType.Minion;

        GameObject enemy = enemyPool.GetEnemy(typeToSpawn);
        if (enemy == null) return;

        enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        enemy.SetActive(true);

        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.Initialize(target, enemyPool);
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
