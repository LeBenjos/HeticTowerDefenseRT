using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Tower took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Tower destroyed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}