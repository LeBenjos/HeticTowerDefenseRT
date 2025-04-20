using UnityEngine;

[RequireComponent(typeof(TowerBase))]
public class TowerShooter : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 3f;

    [Header("Attack")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform shootPoint;

    [Header("References")]
    private TowerProjectilePool towerProjectilePool;
    private float fireCooldown;

    private void Awake()
    {
        // Auto-assignation si la référence n’est pas mise à la main
        if (towerProjectilePool == null)
        {
            towerProjectilePool = FindFirstObjectByType<TowerProjectilePool>();
            if (towerProjectilePool == null)
            {
                Debug.LogWarning($"[TowerShooter] Aucun TowerProjectilePool trouvé dans la scène !");
            }
        }
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            EnemyBase target = FindClosestEnemy();

            if (target != null)
            {
                Shoot(target);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    private EnemyBase FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        EnemyBase closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase enemy))
            {
                if (enemy.CurrentState == EnemyState.Moving)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = enemy;
                    }
                }
            }
        }

        return closest;
    }

    private void Shoot(EnemyBase target)
    {
        if (towerProjectilePool == null || shootPoint == null) return;

        GameObject proj = towerProjectilePool.Get();
        proj.transform.SetPositionAndRotation(shootPoint.position, Quaternion.identity);
        proj.SetActive(true);

        proj.GetComponent<TowerProjectile>()?.Initialize(target.transform);
    }
}
