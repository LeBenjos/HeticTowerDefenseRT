using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp = 10;
    private Transform target;
    private EnemyPool pool;
    private Animator animator;

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float despawnDistance = 0.1f;

    private EnemyState currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(Transform target, EnemyPool pool, int hp)
    {
        this.target = target;
        this.pool = pool;
        this.hp = hp;

        SetState(EnemyState.Spawning);
    }

    private void SetState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Spawning:
                animator.Play("EnemySpawn");
                transform.LookAt(target.position);
                break;

            case EnemyState.Moving:
                animator.Play("EnemyRun");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            pool.ReturnEnemy(gameObject);
            GameManager.Instance.AddKill();
        }
    }

    private void Update()
    {
        if (target == null || currentState != EnemyState.Moving) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < despawnDistance)
        {
            pool.ReturnEnemy(gameObject);
        }
    }

    public void OnSpawnAnimationFinished()
    {
        SetState(EnemyState.Moving);
    }
}
