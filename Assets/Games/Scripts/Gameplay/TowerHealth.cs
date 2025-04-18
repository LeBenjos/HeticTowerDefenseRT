using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

	public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

		if (healthBar != null)
		{
    		healthBar.maxValue = maxHealth;
   		 	healthBar.value = currentHealth;
		}
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
		UpdateHealthBar();
        Debug.Log("Tower took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Tower destroyed!");
       	GameManager.Instance.TriggerGameOver();
    }

	void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}