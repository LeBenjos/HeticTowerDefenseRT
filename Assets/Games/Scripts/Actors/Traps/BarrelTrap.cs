using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BarrelTrap : TrapBase
{
    private readonly float explosionRadius = 0.5f;
    private readonly float cooldownDuration = 5f;

    [Header("Visuals")]
    [SerializeField] private GameObject meshToDisable;
    [SerializeField] private ParticleSystem explosionVFX;

    [Header("Audio")]
    public AudioSource explosionAudio;

    private bool isOnCooldown = false;

    protected void Awake()
    {
        damage = 999;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOnCooldown) return;

        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionVFX != null) explosionVFX.Play();
        if (meshToDisable != null) meshToDisable.SetActive(false);

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;

        if (meshToDisable != null) meshToDisable.SetActive(true);
    }

    public override void Activate(GameObject _) => Explode();
}