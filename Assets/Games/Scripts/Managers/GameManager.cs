using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlaceTower towerPlacer;
    public Button startButton;
    public EnemySpawner enemySpawner;

    [Header("Game Settings")]
    [SerializeField] private float initialTimeBetweenSpawns = 2f;  // Temps initial entre les spawns
    [SerializeField] private int initialEnemiesPerWave = 5;       // Nombre initial d'ennemis par vague

    private float gameTime;  // Temps écoulé depuis le début du jeu
    
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

        // Ajuste la difficulté (temps entre les spawns et nombre d'ennemis) en fonction du temps écoulé
        AdjustDifficulty(gameTime);
    }

    public void StartGame()
    {
        Debug.Log("Game started");
        gameTime = 0f;
        towerPlacer.LockPlacement();
        startButton.GetComponent<ButtonVisibility>().OnGameStart();

        // Démarre le spawn continu des ennemis
        SpawnSettings initialSpawnSettings = new SpawnSettings()
        {
            TimeBetweenSpawns = initialTimeBetweenSpawns,
            EnemiesPerWave = initialEnemiesPerWave
        };

        enemySpawner.UpdateSpawnSettings(initialSpawnSettings);
        enemySpawner.StartContinuousSpawn();
    }

    // Ajuste la difficulté au fur et à mesure que le temps de jeu augmente
    private void AdjustDifficulty(float time)
    {
        // Modifie le temps entre les spawns et le nombre d'ennemis par vague
        float adjustedTimeBetweenSpawns = Mathf.Max(0.5f, initialTimeBetweenSpawns - time / 100f);
        int adjustedEnemiesPerWave = Mathf.Min(20, initialEnemiesPerWave + Mathf.FloorToInt(time / 10f));

        // Transmet les nouveaux paramètres au spawner
        SpawnSettings adjustedSpawnSettings = new SpawnSettings()
        {
            TimeBetweenSpawns = adjustedTimeBetweenSpawns,
            EnemiesPerWave = adjustedEnemiesPerWave
        };

        enemySpawner.UpdateSpawnSettings(adjustedSpawnSettings);
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
