using UnityEngine;

public class TeslaLinkLine : MonoBehaviour
{
    private Transform source;
    private Transform target;
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Initialize(Transform source, Transform target)
    {
        this.source = source;
        this.target = target;
    }

    private void Update()
    {
        if (source == null || target == null)
        {
            Destroy(gameObject);
            return;
        }

        line.SetPosition(0, transform.parent.position);
        line.SetPosition(1, target.position);
    }
}