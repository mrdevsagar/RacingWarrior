using UnityEngine;
using TMPro; // For using TextMeshPro

public class BlinkingLoadingText : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // TextMeshPro component
    public float blinkInterval = 0.5f; // Time interval for blinking

    private string baseText = "Loading";
    private int dotCount = 0;

    void OnEnable()
    {
        dotCount = 0;
        CancelInvoke(nameof(UpdateText));
        // Start the blinking using InvokeRepeating when the GameObject is enabled
        InvokeRepeating(nameof(UpdateText), 0f, blinkInterval);
    }

    void OnDisable()
    {
        Debug.Log("end");
        // Stop the blinking using CancelInvoke when the GameObject is disabled
        CancelInvoke(nameof(UpdateText));
    }

    void UpdateText()
    {
        // Update the text based on the current dot count
        string newText = baseText + new string('.', dotCount);
        if (tmpText != null)
        {
            tmpText.text = newText;
        }

        // Update the dot count for the next iteration
        dotCount = (dotCount + 1) % 4;
    }
}
