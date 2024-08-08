using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float sizeChangeFactor = 0.9f; // Factor by which the size changes when clicked
    public float animationDuration = 0.1f; // Duration of the size change animation
    public float clickAnimationDuration = 0.05f; // Duration of the size change animation for the click event

    private Vector3 originalScale;
    private Button button;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        originalScale = transform.localScale;
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            StartScaling(originalScale * sizeChangeFactor, animationDuration);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            StartScaling(originalScale, animationDuration);
        }
    }

    private void OnButtonClick()
    {
        if (button.interactable)
        {
            StartScaling(originalScale * sizeChangeFactor, clickAnimationDuration, true);
        }
    }

    private void StartScaling(Vector3 targetScale, float duration, bool resetScale = false)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScaleToSize(targetScale, duration, resetScale));
    }

    private System.Collections.IEnumerator ScaleToSize(Vector3 targetScale, float duration, bool resetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        if (resetScale)
        {
            yield return new WaitForSeconds(duration);
            currentCoroutine = StartCoroutine(ScaleToSize(originalScale, duration,false));
        }
    }
}
