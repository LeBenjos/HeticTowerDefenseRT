public class EnemyBoss : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Boss;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 50;
        currentHp = 50;
        speed = 0.1f;
        despawnDistance = 0.05f;
    }
}