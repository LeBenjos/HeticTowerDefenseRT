using UnityEngine;

public class CanonTrap : TrapBase
{
    [Header("Detection")]
    private readonly float detectionRadius = 1f;

    [Header("Attack")]
    private readonly float fireRate = 2f;
    [SerializeField] private Transform shootPoint;

    [Header("Firing Arc")]
    private readonly float maxFiringAngle = 90f;

    [Header("References")]
    private TrapProjectilePool projectilePool;

    private float fireCooldown;

    [Header("Visual")]
    [SerializeField] private Transform rotatingHead;

    private readonly float rotationSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource shootAudio;

    private void Awake()
    {
        if (projectilePool == null)
        {
            projectilePool = FindFirstObjectByType<TrapProjectilePool>();
        }
    }

    private void Update()
    {
        if (!enabled) return;

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
    }

    private EnemyBase FindClosestEnemyInFiringArc()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        EnemyBase closest = null;
        float minDistance = Mathf.Infinity;

        Vector3 forward = transform.forward;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out EnemyBase enemy))
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

    public override void Activate(GameObject _) { }
}
