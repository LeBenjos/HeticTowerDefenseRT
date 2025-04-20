using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class EnemyAttack : MonoBehaviour
{
    private TowerBase tower;
    private EnemyBase enemyBase;
    private bool isAttacking = false;

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower") && other.TryGetComponent(out TowerBase t))
        {
            tower = t;
            isAttacking = true;
            enemyBase.SetState(EnemyState.Attacking);
        }
    }

    public void OnAttackAnimationHit()
    {
        if (enemyBase == null || enemyBase.CurrentState != EnemyState.Attacking)
            return;

        if (tower != null)
        {
            tower.TakeDamage(enemyBase.DamageAmount);
        }
    }
}
