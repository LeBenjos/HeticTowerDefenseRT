using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Spike(other.GetComponent<EnemyBase>());
        }
    }

    void Spike(EnemyBase enemy)
    {
        enemy.TakeDamage(15);
    }
}
