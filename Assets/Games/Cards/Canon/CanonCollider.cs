using UnityEngine;

public class CanonCollider : MonoBehaviour
{
    public float detectionRadius = 0.5f;
    public GameObject canonBullet;
    public GameObject bulletSpawn;

    [SerializeField] float fireCooldown = 3f;
    private float currentCooldown = 0f;


    private Collider DetectEnemy()
    {
        float detectionRadius = 0.5f;
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                    
                    return closestEnemy;
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
            ShootBullet(isEnemy);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
