using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTrap : TrapBase
{
    [Header("Tesla Settings")]
    [SerializeField] private GameObject linkLinePrefab;
    [SerializeField] private Transform linkParent;
    private readonly Dictionary<EnemyBase, GameObject> activeLinks = new();

    private readonly float damageInterval = 1f;
    private readonly float slowFactor = 0.5f;
    private readonly Dictionary<EnemyBase, float> timers = new();

    private readonly float detectionRadius = 0.6f;

    private Color detectionZoneColor = new(0f, 0.95f, 1f, 0.3f);
    private readonly float detectionZoneWidth = 0.005f;

    [Header("Audio")]
    public AudioSource zapAudio;

    private LineRenderer detectionZoneRenderer;

    public void Awake()
    {
        damage = 1;
        SetupDetectionZone();
    }

    private void Update()
    {
        CleanupDeadEnemies();
        UpdateDetectionZone();
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
                if (zapAudio != null)
                {
                    zapAudio.Play();
                }
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

    // -------- Zone visuelle AR ----------

    private void SetupDetectionZone()
    {
        detectionZoneRenderer = gameObject.AddComponent<LineRenderer>();
        detectionZoneRenderer.loop = true;
        detectionZoneRenderer.useWorldSpace = true;
        detectionZoneRenderer.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = detectionZoneColor
        };
        detectionZoneRenderer.startWidth = detectionZoneWidth;
        detectionZoneRenderer.endWidth = detectionZoneWidth;
        detectionZoneRenderer.positionCount = 50;
        DrawDetectionZone();
    }

    private void DrawDetectionZone()
    {
        float angleStep = 360f / detectionZoneRenderer.positionCount;
        float angle = 0f;

        for (int i = 0; i < detectionZoneRenderer.positionCount; i++)
        {
            float x = transform.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * detectionRadius;
            float z = transform.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * detectionRadius;
            detectionZoneRenderer.SetPosition(i, new Vector3(x, transform.position.y, z));
            angle += angleStep;
        }
    }

    private void UpdateDetectionZone()
    {
        DrawDetectionZone();
    }

    public override void Activate(GameObject _) { }
}
