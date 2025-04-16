using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Transform target;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenSpawns = 1f;

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 circlePos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + target.position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.GetComponent<Enemy>().Initialize(target);
    }
}
