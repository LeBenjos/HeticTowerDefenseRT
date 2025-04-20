using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    private readonly float speed = 1f;
    private readonly int damage = 25;

    private Transform target;
    private TowerProjectilePool pool;

    public void Initialize(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetPool(TowerProjectilePool pool)
    {
        this.pool = pool;
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Deactivate();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled || target == null || !target.gameObject.activeInHierarchy)
            return;

        // Ne touche que sa cible directe
        if (other.transform != target) return;

        if (target.TryGetComponent<EnemyBase>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }

        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        pool?.Return(gameObject);
    }
}
