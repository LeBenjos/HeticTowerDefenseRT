using UnityEngine;
using System.Collections.Generic;

public class TeslaTrap : MonoBehaviour
{
    [SerializeField] private float slowPercentage = 0.5f;
    [SerializeField] private float radius = 0.5f;

    // Pour garder la trace des ennemis ralentis
    private HashSet<EnemyBase> slowedEnemies = new HashSet<EnemyBase>();

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        HashSet<EnemyBase> currentlyDetected = new HashSet<EnemyBase>();

        foreach (Collider hit in hits)
    {
        Debug.Log("Collider détecté : " + hit.name);
        if (hit.CompareTag("Enemy"))
        {
            Debug.Log("ENNEMI DÉTECTÉ : " + hit.name);

            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                currentlyDetected.Add(enemy);

                if (!slowedEnemies.Contains(enemy))
                {
                    enemy.ModifySpeed(slowPercentage);
                    slowedEnemies.Add(enemy);
                    Debug.Log("TeslaTrap → Ennemi ralenti : " + enemy.name);
                }
            }
        }
    }
        var enemiesToReset = new List<EnemyBase>(slowedEnemies);
        foreach (EnemyBase enemy in enemiesToReset)
        {
            if (!currentlyDetected.Contains(enemy))
            {
                enemy.ResetSpeed();
                slowedEnemies.Remove(enemy);
                Debug.Log("TeslaTrap → Ennemi sorti de la zone : " + enemy.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
