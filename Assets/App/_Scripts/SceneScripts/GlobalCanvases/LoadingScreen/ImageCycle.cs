using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycle : MonoBehaviour
{
    public List<Sprite> images; // List of images to cycle through
    public Image displayImage; // Image component to display the images

    private int currentIndex = 0;

    void OnEnable()
    {
        if (images == null || images.Count == 0)
        {
            Debug.LogError("Image list is empty!");
            return;
        }

        if (displayImage == null)
        {
            Debug.LogError("Display Image component is not assigned!");
            return;
        }

        // Set the image when the GameObject is enabled
        displayImage.sprite = images[currentIndex];

        // Update the index for the next image
        currentIndex = (currentIndex + 1) % images.Count;
    }
}
