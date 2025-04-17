using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TowerHealth tower = other.GetComponent<TowerHealth>();
            if (tower != null)
            {
                tower.TakeDamage(damageAmount);
            }
        }
    }
}