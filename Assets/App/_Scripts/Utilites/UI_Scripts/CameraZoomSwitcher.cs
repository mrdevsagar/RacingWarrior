using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using TMPro;

public class CameraZoomSwitcher : MonoBehaviour
{
    public CinemachinePositionComposer positionComposer;  // Updated to use CinemachineCamera
    public Button zoomButton;

    private float[] cameraDistances = { 80f, 100f, 110f, 125f };  // Distances you want to switch between
    private string[] zoomTexts = { "1x", "2x", "3x", "4x" };
    private int currentDistanceIndex = 0;

    private CinemachineComposer composer;

    void Start()
    {
        zoomButton.onClick.AddListener(OnZoomButtonClick);
        UpdateCameraZoom();
    }

    void OnZoomButtonClick()
    {

        // Cycle through the camera distance levels
        currentDistanceIndex = (currentDistanceIndex + 1) % cameraDistances.Length;
        UpdateCameraZoom();
    }

    void UpdateCameraZoom()
    {
        if (positionComposer != null)
        {
            Debug.LogError(cameraDistances[currentDistanceIndex]);
            positionComposer.CameraDistance = cameraDistances[currentDistanceIndex];

            // Update the button text to match the zoom level
            zoomButton.GetComponentInChildren<TextMeshProUGUI>().text = zoomTexts[currentDistanceIndex];
        }
    }
}
