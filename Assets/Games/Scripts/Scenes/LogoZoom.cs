using UnityEngine;
using UnityEngine.UI;

public class LogoZoom : MonoBehaviour
{
    public float duration = 5f;         // Durée du zoom
    public float zoomFactor = 1.2f;     // Zoom max
    public float fadeDuration = 1f;     // Durée du fade-in

    private Vector3 initialScale;
    private Vector3 targetScale;
    private float elapsed = 0f;
    private Image image;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale * zoomFactor;

        image = GetComponent<Image>();

        if (image != null)
        {
            Color c = image.color;
            c.a = 0f; // Démarrer invisible
            image.color = c;
        }
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

        if (image != null && elapsed <= fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }
    }
}
