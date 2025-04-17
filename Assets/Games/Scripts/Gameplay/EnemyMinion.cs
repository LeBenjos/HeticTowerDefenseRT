public class EnemyMinion : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Minion;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 10;
        currentHp = 10;
        speed = 0.5f;
        despawnDistance = 0.1f;
    }
}