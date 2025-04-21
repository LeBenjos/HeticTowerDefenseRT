using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    private readonly float speed = 2f;
    private readonly int damage = 50;

    private Transform target;
    private TrapProjectilePool pool;

    public void Initialize(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetPool(TrapProjectilePool pool)
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

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(damage);
            }

            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        pool?.Return(gameObject);
    }
}