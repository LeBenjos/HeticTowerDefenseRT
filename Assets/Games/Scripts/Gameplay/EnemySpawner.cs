using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemyPool enemyPool;
    private Transform target;
    [SerializeField] private float spawnRadius = 3f;

    private float timeBetweenSpawns;  // Temps entre chaque spawn
    private int enemiesPerWave;       // Nombre d'ennemis à faire apparaître à chaque fois
    private float spawnTimer;         // Timer pour gérer le délai de spawn
    private bool isSpawning = false;  // Booléen pour contrôler si les ennemis sont en train de spawner

    void Start()
    {
        spawnTimer = 0f;  // Initialiser le timer à 0
    }

    void Update()
    {
        if (!isSpawning)
            return;

        // Si le timer est à 0, il est temps de spawn les ennemis
        if (spawnTimer <= 0f)
        {
            SpawnEnemies();  // Spawn les ennemis
            spawnTimer = timeBetweenSpawns;  // Réinitialiser le timer pour le prochain spawn
        }
        else
        {
            spawnTimer -= Time.deltaTime;  // Réduire le timer à chaque frame
        }
    }

    // Faire apparaître les ennemis
    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)  // Spawn plusieurs ennemis par vague
        {
            SpawnEnemy();
        }
    }

    // Faire apparaître un ennemi individuel
    private void SpawnEnemy()
    {
        Vector2 circlePos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + target.position;

        GameObject enemy = enemyPool.GetEnemy();
        enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().Initialize(target, enemyPool); // On passe le pool pour pouvoir se relâcher ensuite
    }

    // Définir la cible des ennemis (la position de la tour)
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Mettre à jour les paramètres de spawn (temps entre chaque spawn, nombre d'ennemis)
    public void UpdateSpawnSettings(SpawnSettings newSettings)
    {
        timeBetweenSpawns = newSettings.TimeBetweenSpawns;
        enemiesPerWave = newSettings.EnemiesPerWave;
    }

    // Démarrer le spawn continu des ennemis
    public void StartContinuousSpawn()
    {
        isSpawning = true;
    }

    // Arrêter le spawn continu des ennemis
    public void StopContinuousSpawn()
    {
        isSpawning = false;
    }
}
