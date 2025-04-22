using System.Threading;
using UnityEngine;

public class OldAttackEnemy : MonoBehaviour
{
    [SerializeField] private float fireCooldown = 3;
    private float bulletReset;
    public GameObject canon;
    public Transform spawnPoint;
    private Collider DetectEnemy()
    {
        float detectionRadius = 0.5f;
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
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
        bulletReset -= Time.deltaTime;

        if (bulletReset <= 0) return;
        {
            bulletReset = fireCooldown;

            GameObject bulletObject = Instantiate(canon, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody bulletRigidBody = bulletObject.GetComponent<Rigidbody>();
            bulletRigidBody.AddForce(bulletRigidBody.transform.forward * 10);
            Destroy(bulletObject, 0.1f);


        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShootBullet();

    }
}
