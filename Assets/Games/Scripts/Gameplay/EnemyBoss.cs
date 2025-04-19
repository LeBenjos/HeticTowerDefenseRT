public class EnemyBoss : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Boss;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 150;
        currentHp = 150;
        speed = 3f;
        damageAmount = 100;
        despawnDistance = 0.05f;
    }
}