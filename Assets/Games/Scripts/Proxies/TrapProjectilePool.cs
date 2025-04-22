using System.Collections.Generic;
using UnityEngine;

public class TrapProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 15;

    private readonly Queue<GameObject> pool = new();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab, transform);
            proj.SetActive(false);

            if (proj.TryGetComponent<TrapProjectile>(out var trapProj))
            {
                trapProj.SetPool(this);
            }

            pool.Enqueue(proj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        GameObject proj = Instantiate(projectilePrefab, transform);
        proj.SetActive(false);

        if (proj.TryGetComponent<TrapProjectile>(out var trapProj))
        {
            trapProj.SetPool(this);
        }

        return proj;
    }

    public void Return(GameObject proj)
    {
        proj.SetActive(false);
        pool.Enqueue(proj);
    }
}