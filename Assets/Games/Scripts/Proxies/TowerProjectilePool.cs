using System.Collections.Generic;
using UnityEngine;

public class TowerProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 5;

    private readonly Queue<GameObject> pool = new();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab, transform);
            proj.SetActive(false);

            if (proj.TryGetComponent<TowerProjectile>(out var p)) p.SetPool(this);

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
        proj.GetComponent<TowerProjectile>()?.SetPool(this);
        return proj;
    }

    public void Return(GameObject proj)
    {
        pool.Enqueue(proj);
    }
}