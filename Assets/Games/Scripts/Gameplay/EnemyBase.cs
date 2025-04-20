using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class EnemyBase : MonoBehaviour
{
    public abstract EnemyType EnemyType { get; }

    protected Transform target;
    protected EnemyPool pool;
    protected Animator animator;
    protected int damageAmount = 1;

    protected int maxHp = 1;
    protected int currentHp = 1;
    protected float speed = 1f;
    protected float despawnDistance = 1f;

    protected EnemyState currentState;
    protected float baseSpeed;
    protected float currentSpeed;

    public virtual void Start()
    {
        currentSpeed = speed;
        baseSpeed = speed;
    }

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

    protected virtual void OnEnable()
    {
        GameManager.OnGameOver += HandleGameOver;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    protected virtual void HandleGameOver()
    {
        SetState(EnemyState.Idle);
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
            case EnemyState.Idle:
                animator.Play("EnemyDance");
                break;
            case EnemyState.Dead:
                animator.Play("EnemyDeath");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            SetState(EnemyState.Dead);
            GameManager.Instance.AddKill();
        }
    }

    public void ModifySpeed(float percentage)
    {
        currentSpeed = baseSpeed * percentage;
    }

    public void ResetSpeed()
    {
        currentSpeed = baseSpeed;
    }

    protected virtual void Update()
    {
        if (target == null || currentState != EnemyState.Moving) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * currentSpeed * 0.01f);
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

    public void OnDeathAnimationFinished()
    {
        pool.ReturnEnemy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            if (other.TryGetComponent<TowerHealth>(out var tower))
            {
                tower.TakeDamage(damageAmount);
            }
        }
    }
}
