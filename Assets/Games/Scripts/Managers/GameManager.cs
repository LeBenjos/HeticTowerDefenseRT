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

    public void AddKill()
    {
        if (isGameOver) return;
        enemyKillCount++;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
		int score = (int)CalculateScore(gameTime, enemyKillCount);

		int highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

		bool isNewHighScore = score > highScore;

		if (isNewHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save(); 
        }

        gameOverScreen.SetActive(true);
        timeText.text = $"Time Survived: {gameTime:F1}s";
        killText.text = $"Enemies Killed: {enemyKillCount}";
		scoreText.text = $"Score: {score}/100";

		if (isNewHighScore)
        {
            highScoreText.text = $"NEW HIGH SCORE: {score}/100";

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
            highScoreText.text = $"Your Highest Score: {highScore}/100";
        }

        OnGameOver?.Invoke();
    }

	public float CalculateScore(float playTimeInSeconds, int enemyKillCount)
    {
        const float MAX_PLAY_TIME = 7200f; 
        const int MAX_ZOMBIES = 1000;
    
        const float PLAY_TIME_WEIGHT = 0.5f; 
       	const float ZOMBIES_WEIGHT = 0.5f; 

        float playTimeScore = Mathf.Clamp01(playTimeInSeconds / MAX_PLAY_TIME);
        float zombieScore = Mathf.Clamp01((float)enemyKillCount / MAX_ZOMBIES);
    
    
        float totalScore = (playTimeScore * PLAY_TIME_WEIGHT + zombieScore * ZOMBIES_WEIGHT) * 100f;
    
        return Mathf.Round(totalScore);
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
}
