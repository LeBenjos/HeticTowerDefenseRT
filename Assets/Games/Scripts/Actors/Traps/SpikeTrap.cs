using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpikeTrap : TrapBase
{
    protected float damageInterval = 6f;
    protected void Awake()
    {
        damage = 25;
    }

    private readonly Dictionary<EnemyBase, float> damageTimers = new();

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (enemy.CurrentState != EnemyState.Moving && enemy.CurrentState != EnemyState.Attacking) return;

            if (!damageTimers.TryGetValue(enemy, out float lastHitTime))
            {
                Activate(enemy.gameObject);
                damageTimers[enemy] = Time.time;
            }
            else if (Time.time - lastHitTime >= damageInterval)
            {
                Activate(enemy.gameObject);
                damageTimers[enemy] = Time.time;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            damageTimers.Remove(enemy);
        }
    }

    public override void Activate(GameObject enemyObject)
    {
        if (enemyObject.TryGetComponent(out EnemyBase enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
