using UnityEngine;

public class EnemyRunner : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Runner;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 15;
        currentHp = 15;
        speed = 0.15f;
        despawnDistance = 0.05f;
        damageAmount = 10;
    }
}
