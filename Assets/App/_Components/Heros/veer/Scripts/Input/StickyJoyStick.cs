using UnityEngine;
using UnityEngine.EventSystems;

public class StickyJoystick : Joystick
{
    private Vector2 lastPosition;

    protected override void HandleInput(float magnitude, Vector2 normalized, Vector2 radius, Camera cam)
    {
        if (magnitude > 1)
        {
            // Clamp the position to keep within the normalized radius
            normalized = normalized.normalized;
        }

        // Store the last input position to make it "sticky"
        lastPosition = normalized;

        // Use the last position as the input
        base.HandleInput(magnitude, lastPosition, radius, cam);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // Prevent resetting the joystick by not calling the base method
    }
}
