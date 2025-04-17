using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Spike(other.gameObject);
        }
    }

    void Spike(GameObject enemy)
    {
        Destroy(enemy);
    }
}
