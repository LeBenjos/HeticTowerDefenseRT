public class EnemyBoss : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Boss;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 50;
        currentHp = 50;
        speed = 0.03f;
        despawnDistance = 0.05f;
        damageAmount = 100;
    }
}