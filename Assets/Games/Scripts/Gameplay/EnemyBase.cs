using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public abstract EnemyType EnemyType { get; }

    protected Transform target;
    protected EnemyPool pool;
    protected Animator animator;

    protected int maxHp = 1;
    protected int currentHp = 1;
    protected float speed = 1f;
    protected float despawnDistance = 1f;

    protected EnemyState currentState;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Initialize(Transform target, EnemyPool pool)
    {
        this.target = target;
        this.pool = pool;
        currentHp = maxHp;
        SetState(EnemyState.Spawning);
    }

    protected virtual void SetState(EnemyState newState)
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
        currentHp -= damage;
        if (currentHp <= 0)
        {
            pool.ReturnEnemy(gameObject);
            GameManager.Instance.AddKill();
        }
    }

    protected virtual void Update()
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
