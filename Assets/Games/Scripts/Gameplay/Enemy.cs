using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 1.5f);
        transform.LookAt(target.position);
    }
}