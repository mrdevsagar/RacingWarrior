using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class MyOnScreenStick : OnScreenStick
{
    private Vector2 lastPosition;
    private bool isDragging = false;

    [SerializeField]
    private RectTransform childImageRectTransform;
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        isDragging = true;

        base.OnDrag(eventData);
        UpdateJoystickPosition(eventData);

    }

    public override void OnDrag(PointerEventData eventData)
    {

        base.OnDrag(eventData);
        UpdateJoystickPosition(eventData);

    }



    public override void OnPointerUp(PointerEventData eventData)
    {
       /* base.OnPointerUp(eventData);*/
        isDragging = false;
        lastPosition = transform.localPosition;
        // Optionally reset to center or another position
        // transform.localPosition = Vector2.zero; // Uncomment if you want to reset the knob position
    }

    private void Update()
    {
       /* // If not dragging, maintain the knob at the last position
        if (!isDragging)
        {
            transform.localPosition = lastPosition;
        }*/
    }

    private void UpdateJoystickPosition(PointerEventData eventData)
    {
        // Convert touch position to local space and update the joystick position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        // Clamp the position to stay within the defined movement range from OnScreenStick
        localPoint = Vector2.ClampMagnitude(localPoint, movementRange);

        transform.localPosition = localPoint;

        // Invert the localPoint for the child image's position
        Vector2 invertedPosition = -localPoint;

        // Set the child's position to the inverted joystick position
        childImageRectTransform.localPosition = invertedPosition;

        // The value can be read from the stick's built-in value
        Vector2 inputDirection = new Vector2(localPoint.x / movementRange, localPoint.y / movementRange);
        // Use inputDirection as needed in your game logic (e.g., movement)
    }
}
