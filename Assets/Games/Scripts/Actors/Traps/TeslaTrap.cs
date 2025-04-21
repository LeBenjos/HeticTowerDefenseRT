using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTrap : TrapBase
{
    [Header("Tesla Settings")]
    [SerializeField] private GameObject linkLinePrefab;
    [SerializeField] private Transform linkParent;
    private readonly Dictionary<EnemyBase, GameObject> activeLinks = new();

    private readonly float damageInterval = 2f;
    private readonly float slowFactor = 0.5f;
    private readonly Dictionary<EnemyBase, float> timers = new();
    public void Awake()
    {
        damage = 1;
    }

    private void Update()
    {
        CleanupDeadEnemies();
    }

    private void CleanupDeadEnemies()
    {
        List<EnemyBase> toRemove = new();

        foreach (var pair in activeLinks)
        {
            EnemyBase enemy = pair.Key;

            if (enemy == null || enemy.CurrentState == EnemyState.Dead)
            {
                Destroy(pair.Value);
                toRemove.Add(enemy);
            }
        }

        foreach (var enemy in toRemove)
        {
            activeLinks.Remove(enemy);
            timers.Remove(enemy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (!timers.ContainsKey(enemy))
            {
                timers.Add(enemy, Time.time);
                enemy.ModifySpeed(slowFactor);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;


        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (!activeLinks.ContainsKey(enemy))
            {
                GameObject link = Instantiate(linkLinePrefab, linkParent);
                link.GetComponent<TeslaLinkLine>().Initialize(transform, enemy.transform);
                activeLinks[enemy] = link;
            }

            if (enemy.CurrentState != EnemyState.Moving && enemy.CurrentState != EnemyState.Attacking) return;

            if (!timers.ContainsKey(enemy))
            {
                timers.Add(enemy, Time.time);
                enemy.ModifySpeed(slowFactor);
            }

            float lastHit = timers[enemy];

            if (Time.time - lastHit >= damageInterval)
            {
                enemy.TakeFreeDamage((int)damage);
                timers[enemy] = Time.time;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (activeLinks.TryGetValue(enemy, out var link))
            {
                Destroy(link);
                activeLinks.Remove(enemy);
            }

            if (timers.ContainsKey(enemy))
            {
                enemy.ResetSpeed();
                timers.Remove(enemy);
            }
        }
    }

    public override void Activate(GameObject _) { }
}
