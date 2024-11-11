using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Image displayImage;        // The Image component to display the sprites
    public Sprite[] images;           // Array to hold the images
    public float displayTime = 3f;    // Time each image is displayed before fade-out (in seconds)
    public float fadeDuration = 1f;   // Duration of fade-out (in seconds)

    private Coroutine currentCoroutine;  // Stores the active coroutine
    public PlayerInputHandler input;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        input.OnMoveInputChanged += OnMoveInputChanged;
    }

    private void OnDisable()
    {
        input.OnMoveInputChanged -= OnMoveInputChanged;
    }

    public void OnMoveInputChanged(Vector2 move)
    {
        displayImage.rectTransform.localScale =  new Vector3(move.x, move.y, 1f);
       /* if (move == new Vector2(1, 1))
        {
            ShowImageAtIndex(0);
        }
        else if (move == new Vector2(1, -1))
        {
            ShowImageAtIndex(1);
        }
        else if (move == new Vector2(-1, 1))
        {
            ShowImageAtIndex(2);
        }
        else if (move == new Vector2(-1, -1))
        {
            ShowImageAtIndex(3);
        }
        else
        {
        }*/
    }

    // Method to display the image at the selected index with fade-out effect
    public void ShowImageAtIndex(int index)
    {
        if (images.Length > 0 && index >= 0 && index < images.Length)
        {
            // Stop any ongoing coroutine
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Start a new coroutine for the selected image
            currentCoroutine = StartCoroutine(DisplayImageWithFadeOut(index));
        }
    }

    private IEnumerator DisplayImageWithFadeOut(int index)
    {
        // Display the selected image
        displayImage.sprite = images[index];
        displayImage.color = new Color(1f, 1f, 1f, 1f);  // Fully visible

        // Wait for the specified display time
        float elapsedTime = 0f;
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Start the fade-out effect
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            displayImage.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure fully transparent after fade
        displayImage.color = new Color(1f, 1f, 1f, 0f);
    }
}
