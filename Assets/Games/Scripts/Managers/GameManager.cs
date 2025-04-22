using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public PlaceTower towerPlacer;
    public Button startButton;
    public EnemySpawner enemySpawner;
    public ARPlaneManager planeManager;

    [Header("Game Settings")]
    [SerializeField] private float initialTimeBetweenSpawns = 5f;
    [SerializeField] private int initialEnemiesPerWave = 1;
    private float gameTime;
    private bool isGameOver = false;
    private int enemyKillCount = 0;
    private int enemyMinionKillCount = 0;
    private int enemyRunnerKillCount = 0;
    private int enemyBossKillCount = 0;
	
	private int MINION_SCORE_WEIGHT = 1;
    private int RUNNER_SCORE_WEIGHT = 3;
	private int BOSS_SCORE_WEIGHT = 5;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("High Score Animation")]
    public HighScoreAnimator highScoreAnimator;
    public GameObject confettiParticleSystem;

    private const string HIGH_SCORE_KEY = "HighScore";

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

        if (highScoreAnimator == null && highScoreText != null)
        {
            highScoreAnimator = highScoreText.GetComponent<HighScoreAnimator>();

            if (highScoreAnimator == null)
            {
                highScoreAnimator = highScoreText.gameObject.AddComponent<HighScoreAnimator>();
            }
        }
    }

    void Start()
    {
        gameTime = 0f;
        // ResetHighScore();
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

    public void AddKill(EnemyType enemyType)
    {
        if (isGameOver) return;
        if (enemyType == EnemyType.Minion) enemyMinionKillCount++;
        else if (enemyType == EnemyType.Runner) enemyRunnerKillCount++;
        else if (enemyType == EnemyType.Boss) enemyBossKillCount++;
        enemyKillCount++;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        float score = CalculateScore();

        float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0);

        bool isNewHighScore = score > highScore;

        if (isNewHighScore)
        {
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
        }

        gameOverScreen.SetActive(true);
        timeText.text = $"Time Survived: {gameTime:F1}s";
        killText.text = $"Enemies Killed: {enemyKillCount}";
        scoreText.text = $"Score: {score:F0}";

        if (isNewHighScore)
        {
            highScoreText.text = $"NEW HIGH SCORE: {score:F0}";

            if (highScoreAnimator != null)
            {
                highScoreAnimator.PlayNewHighScoreAnimation();
            }

            if (confettiParticleSystem != null)
            {
                confettiParticleSystem.SetActive(true);
            }
        }
        else
        {
            highScoreText.text = $"Your Highest Score: {highScore:F0}";
        }

        OnGameOver?.Invoke();
    }

    public float CalculateScore()
    {
		int enemyPoints = CalculateEnemyPoints();
        float totalScore = (gameTime * 0.25f * (float)enemyPoints);

        return totalScore;
    }

	public int CalculateEnemyPoints()
	{	
		int minionPoints = enemyMinionKillCount * MINION_SCORE_WEIGHT;
		int runnerPoints = enemyMinionKillCount * RUNNER_SCORE_WEIGHT;
		int bossPoints = enemyMinionKillCount * BOSS_SCORE_WEIGHT;
		return (minionPoints + runnerPoints + bossPoints);
	}

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
        PlayerPrefs.Save();
        Debug.Log("High score has been reset");
    }

    public void StopPlaneDetection()
    {
        planeManager.enabled = false;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
