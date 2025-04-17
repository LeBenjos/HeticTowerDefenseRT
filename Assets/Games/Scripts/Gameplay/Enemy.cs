using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private EnemyPool pool;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float dispawnDistance = 0.1f;

    public void Start()
    {
        Debug.Log("Create enemy");
    }

    public void Initialize(Transform target, EnemyPool pool)
    {
        this.target = target;
        this.pool = pool;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < dispawnDistance)
        {
            pool.ReturnEnemy(gameObject);
        }
    }
}