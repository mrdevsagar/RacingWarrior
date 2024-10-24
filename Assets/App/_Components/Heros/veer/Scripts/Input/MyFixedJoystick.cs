using UnityEngine;
using UnityEngine.EventSystems;

public class MyFixedJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 lastPosition; // Store the last position of the joystick knob
    public RectTransform knobRectTransform; // Reference to the joystick knob
    public float radius = 50f; // Maximum distance from the center to the knob
    private bool isTouching; // Track if the player is currently touching the knob
    public bool isFiring; // Flag to indicate whether firing is active
    float distance;
    float angle;
    private void Start()
    {
        lastPosition = knobRectTransform.anchoredPosition; // Initialize with the starting position
/*        radius = GetComponent<RectTransform>().anchoredPosition.x / 2;*/
        isFiring = false; // Initialize firing to false
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*// Calculate the new position of the joystick knob based on drag
        Vector2 newPosition = lastPosition + eventData.delta;

        // Clamp the position within a certain radius
        if (newPosition.magnitude >= radius)
        {
            newPosition = newPosition.normalized * radius;

            // Set isFiring to true when max distance is reached
            if (isTouching)
            {
                isFiring = true;
            }
        }
        else
        {
            isFiring = false;
        }

        // Log the distance and angle before updating the knob's position
        LogDistanceAndAngle(newPosition);

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        // Update last position to the new position
        lastPosition = newPosition;*/

        // Convert the screen touch position to the local position relative to the joystick's parent (usually a canvas)
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            knobRectTransform.parent as RectTransform, // The parent RectTransform
            eventData.position,                        // The screen position of the touch
            eventData.pressEventCamera,                // The camera used to convert the screen point
            out localPoint                             // Output the local position
        );

        // Calculate the new position of the joystick knob based on the local point
        Vector2 newPosition = localPoint;

        // Clamp the position within a certain radius
        if (newPosition.magnitude >= radius)
        {
            newPosition = newPosition.normalized * radius;
            isFiring = true; // Set firing if touched outside radius
        }

        // Log the distance and angle before updating the knob's position
        LogDistanceAndAngle(newPosition);

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        lastPosition = newPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        isFiring = false; // Reset firing when the touch is released
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Player is touching the knob
        isTouching = true; // Set to true when the knob is pressed

        // Convert the screen touch position to the local position relative to the joystick's parent (usually a canvas)
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            knobRectTransform.parent as RectTransform, // The parent RectTransform
            eventData.position,                        // The screen position of the touch
            eventData.pressEventCamera,                // The camera used to convert the screen point
            out localPoint                             // Output the local position
        );

        // Calculate the new position of the joystick knob based on the local point
        Vector2 newPosition = localPoint;

        // Clamp the position within a certain radius
        if (newPosition.magnitude >= radius)
        {
            newPosition = newPosition.normalized * radius;
            isFiring = true; // Set firing if touched outside radius
        }

        // Log the distance and angle before updating the knob's position
        LogDistanceAndAngle(newPosition);

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        lastPosition = newPosition;
    }

    private void LogDistanceAndAngle(Vector2 position)
    {
        // Calculate distance from the center
         distance = position.magnitude;

        // Calculate angle in degrees and normalize to [0, 360]
         angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360; // Ensure the angle is positive

        // Log the distance and angle
       /* Debug.Log($"Distance: {distance}, Angle: {angle}");*/
    }

    public void Update()
    {
        Debug.Log(isFiring +"  "+ distance + "  " + angle);
    }
    public void ResetJoystick()
    {
        // Optional: Call this method to reset the joystick back to the center
        lastPosition = Vector2.zero; // Reset last position to zero
        knobRectTransform.anchoredPosition = Vector2.zero; // Reset knob to center
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: Implement any logic for when dragging ends
    }
}
