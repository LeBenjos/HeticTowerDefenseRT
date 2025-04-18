using UnityEngine;

public class CanonBullet : MonoBehaviour
{
    public int speed = 1;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 3f);
    }
}
