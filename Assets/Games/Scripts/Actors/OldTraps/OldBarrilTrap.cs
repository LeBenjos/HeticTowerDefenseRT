using UnityEngine;

public class OldBarrilTrap : OldTrapBase

{
    public float forceExplosion = 700f;
    public float radiusExplosion = 0.4f;
    // public GameObject explosionEffectPrefab; // (Optionnel pour un effet visuel)

    protected int damageAmount = 250;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
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
            EnemyBase enemy = nearbyObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
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