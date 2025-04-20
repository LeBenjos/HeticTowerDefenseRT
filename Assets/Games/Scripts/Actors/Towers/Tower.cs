using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Tower : MonoBehaviour
{
    private readonly int maxHealth = 1000;
    private int currentHealth;
    public Slider healthBar;
    private float targetHealth;
    public float lerpSpeed = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        targetHealth = currentHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (Mathf.Abs(targetHealth - healthBar.value) > 0.1f)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealth, Time.deltaTime * lerpSpeed);
        }
        else
        {
            healthBar.value = targetHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetHealth = currentHealth;

        Debug.Log($"Tower took damage: {amount}. Current health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.TriggerGameOver();
    }
}