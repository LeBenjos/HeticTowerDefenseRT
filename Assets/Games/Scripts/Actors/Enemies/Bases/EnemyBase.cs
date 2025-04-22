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
    protected float attackCooldown = 1f;
    public float AttackCooldown => attackCooldown;
    protected int damageAmount = 1;
    public int DamageAmount => damageAmount;

    protected EnemyState currentState;
    public EnemyState CurrentState => currentState;
    private EnemyState stateBeforeStun = EnemyState.Moving;
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

    public virtual void SetState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Spawning:
                OnSpawning();
                break;
            case EnemyState.Moving:
                OnMoving();
                break;
            case EnemyState.Dance:
                OnDance();
                break;
            case EnemyState.Dead:
                OnDead();
                break;
            case EnemyState.Stunned:
                OnStunned();
                break;
            case EnemyState.Attacking:
                OnAttacking();
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Moving || currentState == EnemyState.Attacking)
        {
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
    }

    public void TakeFreeDamage(int damage)
    {
        if (currentState == EnemyState.Moving || currentState == EnemyState.Attacking)
        {
            currentHp -= damage;

            if (currentHp <= 0 && currentState != EnemyState.Dead)
            {
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        SetState(EnemyState.Dead);
        GameManager.Instance.AddKill(EnemyType);
    }

    protected virtual void Stumble()
    {
        stateBeforeStun = currentState;
        SetState(EnemyState.Stunned);
    }

    protected virtual void HandleGameOver()
    {
        SetState(EnemyState.Dance);
    }

    #endregion

    #region State Handlers

    protected virtual void OnSpawning()
    {
        animator.Play(EnemyAnimationNames.Spawn);
        transform.LookAt(target.position);
    }
    protected virtual void OnMoving()
    {
        animator.Play(EnemyAnimationNames.Run);
    }
    protected virtual void OnDance()
    {
        animator.Play(EnemyAnimationNames.Dance);
    }
    protected virtual void OnDead()
    {
        animator.Play(EnemyAnimationNames.Death);
    }
    protected virtual void OnStunned()
    {
        animator.Play(EnemyAnimationNames.Stumble);
    }
    protected virtual void OnAttacking()
    {
        animator.Play(EnemyAnimationNames.Attack);
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
    public void OnStunAnimationFinished() => SetState(stateBeforeStun);
    public void OnDeathAnimationFinished() => pool.Return(gameObject);

    #endregion
}
