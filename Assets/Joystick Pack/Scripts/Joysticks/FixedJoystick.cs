using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 lastPosition; // Store the last position of the joystick knob
    public RectTransform knobRectTransform; // Reference to the joystick knob
    public float radius = 50f;
    private void Start()
    {
        /*knobRectTransform = GetComponent<RectTransform>();*/ // Assuming this script is attached to the knob
        lastPosition = knobRectTransform.anchoredPosition; // Initialize with the starting position
        radius = GetComponent<RectTransform>().anchoredPosition.x/2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the new position of the joystick knob based on drag
        Vector2 newPosition = lastPosition + eventData.delta;

        // Optional: Clamp the position within a certain radius
        // Adjust this value based on your joystick design
        if (newPosition.magnitude > radius)
        {
            newPosition = newPosition.normalized * radius;
        }

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        // Update last position to the new position
        lastPosition = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Do nothing here to keep the knob at the last position
        // You can implement logic here if needed when dragging ends
    }

    public void ResetJoystick()
    {
        // Optional: Call this method to reset the joystick back to the center
        lastPosition = Vector2.zero; // Reset last position to zero
        knobRectTransform.anchoredPosition = Vector2.zero; // Reset knob to center
    }
}
