using UnityEngine;

public class CanonBullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    private Vector3 targetDirection;
    protected int damageAmount = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemyRb != null)
            {
                Debug.Log("Touché !");
                enemyRb.isKinematic = false;
                enemyRb.useGravity = true;

                Vector3 directionToEnemy = (other.transform.position - transform.position).normalized;
                Vector3 forceDirection = -directionToEnemy + Vector3.up * 1.0f;
                forceDirection.Normalize();

                enemyRb.AddForce(forceDirection * 1.5f, ForceMode.Impulse);

            }
            enemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null && targetDirection != Vector3.zero)
        {
            rb.linearVelocity = targetDirection.normalized * speed;
        }
        Destroy(gameObject, 3f);
    }

    public void SetTarget(Vector3 target)
    {
        targetDirection = (target - transform.position).normalized;
    }
}
