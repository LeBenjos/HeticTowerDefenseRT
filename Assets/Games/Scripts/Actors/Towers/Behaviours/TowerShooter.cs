using UnityEngine;

[RequireComponent(typeof(TowerBase))]
public class TowerShooter : MonoBehaviour
{
    [Header("Detection")]
    private readonly float detectionRadius = 1.5f;

    [Header("Attack")]
    private readonly float fireRate = 1f;
    [SerializeField] private Transform shootPoint;
    private TowerProjectilePool towerProjectilePool;
    private float fireCooldown;

    [Header("Audio")]
    [SerializeField] private AudioSource shootAudio;

    private void Awake()
    {
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
                if (enemy.CurrentState == EnemyState.Moving || enemy.CurrentState == EnemyState.Attacking)
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

        if (shootAudio != null)
        {
            shootAudio.Play();
        }

        proj.GetComponent<TowerProjectile>()?.Initialize(target.transform);
    }
}
