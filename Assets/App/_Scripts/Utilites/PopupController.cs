using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f; // Duration of the animation

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 initialScale;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
         
        // Start the popup animation when the panel is activated
        StartCoroutine(AnimatePopup(true));
    }

    private void OnDisable()
    {
        // Optionally, start an animation to hide the panel if needed
        /*StartCoroutine(AnimatePopup(false));*/
        canvasGroup.alpha = 0f;
    }

    private IEnumerator AnimatePopup(bool show)
    {
        float elapsedTime = 0;
        Vector3 targetScale = show ? initialScale : Vector3.zero;
        float targetAlpha = show ? 1 : 0;

        Vector3 startingScale = rectTransform.localScale;
        float startingAlpha = canvasGroup.alpha;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Scale and alpha interpolation
            rectTransform.localScale = Vector3.Lerp(startingScale, targetScale, t);
            canvasGroup.alpha = Mathf.Lerp(startingAlpha, targetAlpha, t);

            yield return null;
        }

        // Ensure the final state is set
        rectTransform.localScale = targetScale;
        canvasGroup.alpha = targetAlpha;
    }
}
