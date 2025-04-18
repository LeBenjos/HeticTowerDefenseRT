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
    private void ShootBullet()
    {   
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
        // GameObject bulletObject = Instantiate(canon, spawnPoint.transform.position, spawnPoint.transform.rotation);
        // Rigidbody bulletRigidBody = bulletObject.GetComponent<Rigidbody>();
        // bulletRigidBody.AddForce(bulletRigidBody.transform.forward * 10);
        // Destroy(bulletObject, 0.1f);
            Instantiate(canonBullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            fireCooldown = 3;
        };



    }
    
    void Update()
    {
        ShootBullet();
        Collider isEnemy = DetectEnemy();
        if(isEnemy != null)
        {
            Enemy enemy = isEnemy.GetComponent<Enemy>();
            Vector3 enemyPosition = enemy.transform.position;
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(15);
            // }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
