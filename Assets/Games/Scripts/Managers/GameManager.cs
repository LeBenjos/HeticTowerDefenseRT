using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public PlaceTower towerPlacer;
    public Button startButton;
    public EnemySpawner enemySpawner;

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

        enemySpawner.UpdateSpawnSettings(
            initialTimeBetweenSpawns,
            initialEnemiesPerWave
        );
        enemySpawner.StartContinuousSpawn();
    }

    private void AdjustDifficulty(float time)
    {
        enemySpawner.UpdateSpawnSettings(
            Mathf.Lerp(initialTimeBetweenSpawns, 0.5f, time / 300f),
            Mathf.FloorToInt(initialEnemiesPerWave + Mathf.Sqrt(time / 5f))
        );
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
        timeText.text = $"Time Survived: {gameTime:F1}s";
        killText.text = $"Enemies Killed: {enemyKillCount}";

        OnGameOver?.Invoke();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
