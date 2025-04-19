using UnityEngine;

public class EnemyRunner : EnemyBase
{
    public override EnemyType EnemyType => EnemyType.Runner;

    protected override void Awake()
    {
        base.Awake();
        maxHp = 30;
        currentHp = 30;
        speed = 15f;
        damageAmount = 15;
        despawnDistance = 0.05f;
    }
}
