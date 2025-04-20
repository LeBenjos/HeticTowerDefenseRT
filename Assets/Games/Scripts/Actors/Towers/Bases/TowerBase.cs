using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public readonly int maxHealth = 1000;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        GameManager.Instance.TriggerGameOver();
    }

    public event System.Action<int, int> OnHealthChanged;
}