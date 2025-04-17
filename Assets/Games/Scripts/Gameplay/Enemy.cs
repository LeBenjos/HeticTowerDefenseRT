using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField] private int hp = 10;
    private Transform target;
    private EnemyPool pool;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float dispawnDistance = 0.1f;

    public void Start()
    {
        Debug.Log("Create enemy");
    }
    
    public void Initialize(Transform target, EnemyPool pool, int hp)
    {
        this.target = target;
        this.pool = pool;
        this.hp = hp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            pool.ReturnEnemy(gameObject);
        }
    }


    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.LookAt(target.position);

        // Si trop proche, on retourne dans le pool
        if (Vector3.Distance(transform.position, target.position) < dispawnDistance)
        {
            pool.ReturnEnemy(gameObject);
        }
        
    }
}