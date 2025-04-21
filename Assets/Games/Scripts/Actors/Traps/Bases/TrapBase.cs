using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected int damage;
    public int Damage => damage;

    public abstract void Activate(GameObject enemy);
}