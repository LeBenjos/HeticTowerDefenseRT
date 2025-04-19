using UnityEngine;

public class SpikeTrap : TrapBase
{
    protected int damageAmount = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GetDamage(other.GetComponent<EnemyBase>());
        }
    }

    void GetDamage(EnemyBase enemy)
    {
        enemy.TakeDamage(damageAmount);
    }
}
