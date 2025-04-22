using UnityEngine;

public class CanonTrap : TrapBase
{
    [Header("Detection")]
    private readonly float detectionRadius = 0.5f;

    [Header("Attack")]
    public float fireRate = 2f;
    public Transform shootPoint;

    [Header("Firing Arc")]
    public float maxFiringAngle = 180f;

    [Header("References")]
    public TrapProjectilePool projectilePool;

    private float fireCooldown;

    [Header("Visual")]
    public Transform rotatingHead;

    public float rotationSpeed = 5f;

    [Header("Audio")]
    public AudioSource shootAudio;

    private LineRenderer detectionZoneRenderer;

    private void Start()
    {
        if (projectilePool == null)
        {
            projectilePool = FindFirstObjectByType<TrapProjectilePool>();
        }

        SetupDetectionZone();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            EnemyBase target = FindClosestEnemyInFiringArc();

            if (target != null)
            {
                Vector3 dir = target.transform.position - rotatingHead.position;
                dir.y = 0f;
                rotatingHead.rotation = Quaternion.LookRotation(dir);
                Shoot(target);
                fireCooldown = 1f / fireRate;
            }
        }

        UpdateDetectionZone();
    }

    private EnemyBase FindClosestEnemyInFiringArc()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        EnemyBase closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                if (enemy.CurrentState != EnemyState.Moving && enemy.CurrentState != EnemyState.Attacking)
                    continue;

                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToEnemy);

                if (angle > maxFiringAngle) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    private void Shoot(EnemyBase target)
    {
        if (projectilePool == null || shootPoint == null || target == null) return;

        GameObject projectile = projectilePool.Get();
        projectile.transform.SetPositionAndRotation(shootPoint.position, Quaternion.identity);
        projectile.SetActive(true);

        if (shootAudio != null)
        {
            shootAudio.Play();
        }

        if (projectile.TryGetComponent(out TrapProjectile trapProj))
        {
            trapProj.Initialize(target.transform);
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
            color = new Color(1f, 0f, 0f, 0.3f)
        };
        detectionZoneRenderer.startWidth = 0.01f;
        detectionZoneRenderer.endWidth = 0.005f;
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
