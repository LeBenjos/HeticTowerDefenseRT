using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroSceneTransition : MonoBehaviour
{
    public Image fadeImage;
    private readonly float delayBeforeFade = 3f;
    private readonly float fadeDuration = 1.5f;
    public string nextSceneName = "ARMain";

    void Start()
    {
        StartCoroutine(FadeToBlackAndLoad());
    }

    IEnumerator FadeToBlackAndLoad()
    {
        // Attendre avant de commencer le fade
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;
        Color c = fadeImage.color;

        // Assure-toi que le panneau est transparent au début
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);

        // Fade transparent → noir
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // Petit délai pour que le fade soit bien terminé
        yield return new WaitForSeconds(0.3f);

        // Charger la prochaine scène
        SceneManager.LoadScene(nextSceneName);
    }
}
