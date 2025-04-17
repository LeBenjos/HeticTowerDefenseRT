using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private bool isGameOver = false;
    private int enemyKillCount = 0;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killText;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameTime = 0f;
    }

    void Update()
    {
        if (isGameOver) return;
        // Augmente le temps de jeu à chaque frame
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

    public void AddKill()
    {
        if (isGameOver) return;
        enemyKillCount++;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        gameOverScreen.SetActive(true);
        timeText.text = $"Time Survived: {gameTime:F1} seconds";
        killText.text = $"Enemies Killed: {enemyKillCount}";

        enemySpawner.StopContinuousSpawn();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
