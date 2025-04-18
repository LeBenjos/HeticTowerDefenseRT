using UnityEngine;

public class CanonBullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    private Vector3 targetDirection;

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
