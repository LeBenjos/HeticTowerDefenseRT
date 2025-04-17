using UnityEngine;

public class ExplosionBaril : MonoBehaviour
{
    public float forceExplosion = 700f;
    public float radiusExplosion = 0.4f;
    // public GameObject explosionEffectPrefab; // (Optionnel pour un effet visuel)

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Touché");
            Exploser();
        }
    }

    void Exploser()
    {
        // if (explosionEffectPrefab != null)
        // {
        //     Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        // }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        foreach (Collider nearbyObject in colliders)
        {   
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {   
                Debug.Log("Mechant");
                enemy.TakeDamage(10);
            }
        }

        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
}