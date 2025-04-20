public class EnemyMinion : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Minion;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 50;
        currentHp = 50;
        speed = 5f;
        damageAmount = 25;
        despawnDistance = 0.05f;
        attackCooldown = 3f;
    }
}