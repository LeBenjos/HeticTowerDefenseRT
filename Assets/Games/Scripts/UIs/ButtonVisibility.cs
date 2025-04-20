using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonVisibility : MonoBehaviour
{
    private Button startButton;
    private Image buttonImage;
    private TMP_Text buttonText;

    void Start()
    {
        startButton = GetComponent<Button>();
        buttonImage = startButton.GetComponent<Image>();
        buttonText = startButton.GetComponentInChildren<TMP_Text>();
        SetButtonVisibility(true);
        startButton.interactable = false;
    }

    public void OnGameStart()
    {
        SetButtonVisibility(false);
    }

    private void SetButtonVisibility(bool isVisible)
    {
        Color color = buttonImage.color;
        color.a = isVisible ? 1f : 0f;
        buttonImage.color = color;

        if (buttonText != null)
        {
            color = buttonText.color;
            color.a = isVisible ? 1f : 0f;
            buttonText.color = color;
        }

        startButton.interactable = isVisible;
    }
}
