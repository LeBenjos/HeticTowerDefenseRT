using UnityEngine;
using TMPro;
using System.Collections;

public class HighScoreAnimator : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;
    
    [Header("Animation Settings")]
    [SerializeField] private float pulseDuration = 0.5f;
    [SerializeField] private float maxScale = 1.3f;
    [SerializeField] private Color highlightColor = new Color(1f, 0.8f, 0f); // Gold color
    [SerializeField] private float colorPulseDuration = 0.8f;
    [SerializeField] private int numPulses = 3;
    
    private Color originalColor;
    
    void Awake()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        originalColor = highScoreText.color;
    }
    
    public void PlayNewHighScoreAnimation()
    {
        // Make sure text is visible
        highScoreText.gameObject.SetActive(true);
        
        // Start the animation coroutine
        StartCoroutine(AnimateHighScore());
    }
    
    private IEnumerator AnimateHighScore()
    {
        // Save original scale
        Vector3 originalScale = highScoreText.transform.localScale;
        
        // Perform multiple pulses
        for (int i = 0; i < numPulses; i++)
        {
            // Scale animation
            float startTime = Time.time;
            while (Time.time < startTime + pulseDuration)
            {
                float progress = (Time.time - startTime) / pulseDuration;
                float scaleFactor = 1 + (maxScale - 1) * Mathf.Sin(progress * Mathf.PI);
                highScoreText.transform.localScale = originalScale * scaleFactor;
                yield return null;
            }
            
            // Color pulse animation
            startTime = Time.time;
            while (Time.time < startTime + colorPulseDuration)
            {
                float progress = (Time.time - startTime) / colorPulseDuration;
                highScoreText.color = Color.Lerp(highlightColor, originalColor, progress);
                yield return null;
            }
            
            // Reset to original color between pulses
            highScoreText.color = originalColor;
            
            // Small pause between pulses
            yield return new WaitForSeconds(0.2f);
        }
        
        // Ensure we end at original scale
        highScoreText.transform.localScale = originalScale;
    }
}