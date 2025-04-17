using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlaceTower towerPlacer;
    public Button startButton;
    public EnemySpawner enemySpawner;

    private readonly SpawnSettings currentSpawnSettings = new();

    [Header("Game Settings")]
    [SerializeField] private float initialTimeBetweenSpawns = 5f;
    [SerializeField] private int initialEnemiesPerWave = 1;

    private float gameTime;

    void Start()
    {
        gameTime = 0f;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        AdjustDifficulty(gameTime);
    }

    public void StartGame()
    {
        gameTime = 0f;
        towerPlacer.LockPlacement();
        startButton.GetComponent<ButtonVisibility>().OnGameStart();

        currentSpawnSettings.TimeBetweenSpawns = initialTimeBetweenSpawns;
        currentSpawnSettings.EnemiesPerWave = initialEnemiesPerWave;
        enemySpawner.UpdateSpawnSettings(currentSpawnSettings);
        enemySpawner.StartContinuousSpawn();
    }

    private void AdjustDifficulty(float time)
    {
        currentSpawnSettings.TimeBetweenSpawns = Mathf.Lerp(initialTimeBetweenSpawns, 0.5f, time / 300f);
        currentSpawnSettings.EnemiesPerWave = Mathf.FloorToInt(initialEnemiesPerWave + Mathf.Sqrt(time / 5f));

        enemySpawner.UpdateSpawnSettings(currentSpawnSettings);
    }
}
