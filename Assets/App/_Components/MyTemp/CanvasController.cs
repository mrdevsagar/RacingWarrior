

using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasController : MonoBehaviour
{
    private bool isRotatingRight = false;
    private bool isRotatingLeft = false;
    public enum RotationDirection { None, Left, Right }
    public RotationDirection currentDirection = RotationDirection.None;

    // Event to send the rotation direction to the second script
    public delegate void RotationAction(RotationDirection direction);
    public static event RotationAction OnRotationChanged;

    void Update()
    {
        // Check which rotation is active
        if (isRotatingRight)
        {
            currentDirection =  RotationDirection.Right;
        } else if (isRotatingLeft)
        {
            currentDirection = RotationDirection.Left;
        }
        else
        {
            currentDirection = RotationDirection.None;
        }

        // Send the direction to the other script
        if (OnRotationChanged != null)
        {
            OnRotationChanged?.Invoke(currentDirection);
        }

       
    }

    public void OnPointerDownLeft()
    {
        // Start rotating based on the button pressed
        isRotatingLeft = true;
    }

    public void OnPointerUpLeft()
    {
        // Stop rotating when the button is released
        isRotatingLeft = false;
    }


    public void OnPointerDownRight()
    {
        // Start rotating based on the button pressed
        isRotatingRight = true;
    }

    public void OnPointerUpRight()
    {
        // Stop rotating when the button is released
        isRotatingRight = false;
    }
}
