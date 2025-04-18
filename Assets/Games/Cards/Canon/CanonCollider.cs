using UnityEngine;

public class CanonCollider : MonoBehaviour
{
    public GameObject canonBullet;
    public GameObject bulletSpawn;

    [SerializeField] float fireCooldown = 3f;
    private float currentCooldown = 0f;

    public float detectionRadius = 10f;
    public float visionAngle = 65f;

    private Collider DetectEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                if (distanceToEnemy <= detectionRadius && angleToEnemy <= visionAngle / 2)
                {
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hit;
                    }
                }
            }
        }

        return closestEnemy;
    }


    private void ShootBullet(Collider enemy)
    {   
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0 && enemy != null)
        {
            GameObject bulletObj = Instantiate(canonBullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            CanonBullet bullet = bulletObj.GetComponent<CanonBullet>();
            if (bullet != null)
            {
                bullet.SetTarget(enemy.transform.position);
            }
            currentCooldown = fireCooldown;
        }
    }

    
    void Update()
    {
            Collider isEnemy = DetectEnemy();
            if(isEnemy != null)
            {
                ShootBullet(isEnemy);
            }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        int segments = 30; 
        float halfAngle = visionAngle / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + visionAngle / segments * i;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * detectionRadius);
        }
    }

}
