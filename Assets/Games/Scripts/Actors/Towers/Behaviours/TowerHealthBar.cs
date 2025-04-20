using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TowerBase))]
public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private float lerpSpeed = 5f;
    private int targetHealth;
    void Start()
    {
        var tower = GetComponent<TowerBase>();
        tower.OnHealthChanged += UpdateHealth;
        UpdateHealth(tower.maxHealth, tower.maxHealth);
    }

    void Update()
    {
        if (healthBar == null) return;

        if (Mathf.Abs(healthBar.value - targetHealth) > 0.1f)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealth, Time.deltaTime * lerpSpeed);
        }
        else
        {
            healthBar.value = targetHealth;
        }
    }

    private void UpdateHealth(int newHealth, int max)
    {
        targetHealth = newHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = max;
        }
    }
}
