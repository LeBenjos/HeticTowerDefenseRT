using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class EnemyBase : MonoBehaviour
{
    public abstract EnemyType EnemyType { get; }

    protected Transform target;
    protected EnemyPool pool;
    protected Animator animator;

    [Header("Stats")]
    protected int maxHp = 1;
    protected int currentHp = 1;
    protected float speed = 1f;
    protected float despawnDistance = 1f;
    protected int damageAmount = 1;

    protected EnemyState currentState;
    public EnemyState CurrentState => currentState;
    protected float baseSpeed;
    protected float currentSpeed;

    #region Unity Methods

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        GameManager.OnGameOver += HandleGameOver;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    protected virtual void Start()
    {
        baseSpeed = speed;
        currentSpeed = speed;
    }

    protected virtual void Update()
    {
        if (target == null || currentState != EnemyState.Moving) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * currentSpeed * 0.01f);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < despawnDistance)
        {
            pool.Return(gameObject);
        }
    }

    #endregion

    #region Core Logic

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
                animator.Play(EnemyAnimationNames.Spawn);
                transform.LookAt(target.position);
                break;
            case EnemyState.Moving:
                animator.Play(EnemyAnimationNames.Run);
                break;
            case EnemyState.Idle:
                animator.Play(EnemyAnimationNames.Dance);
                break;
            case EnemyState.Dead:
                animator.Play(EnemyAnimationNames.Death);
                break;
            case EnemyState.Stunned:
                animator.Play(EnemyAnimationNames.Stumble);
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState != EnemyState.Moving) return;

        currentHp -= damage;

        if (currentHp <= 0 && currentState != EnemyState.Dead)
        {
            Die();
        }
        else
        {
            Stumble();
        }
    }

    protected virtual void Die()
    {
        SetState(EnemyState.Dead);
        GameManager.Instance.AddKill();
    }

    protected virtual void Stumble()
    {
        SetState(EnemyState.Stunned);
    }

    protected virtual void HandleGameOver()
    {
        SetState(EnemyState.Idle);
    }

    #endregion

    #region Speed Modifiers

    public void ModifySpeed(float percentage)
    {
        currentSpeed = baseSpeed * percentage;
    }

    public void ResetSpeed()
    {
        currentSpeed = baseSpeed;
    }

    #endregion

    #region Animation Events

    public void OnSpawnAnimationFinished() => SetState(EnemyState.Moving);
    public void OnStunAnimationFinished() => SetState(EnemyState.Moving);
    public void OnDeathAnimationFinished() => pool.Return(gameObject);

    #endregion

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled || currentState == EnemyState.Dead) return;

        if (other.CompareTag("Tower") && other.TryGetComponent(out TowerBase tower))
        {
            tower.TakeDamage(damageAmount);
        }
    }

    #endregion
}
