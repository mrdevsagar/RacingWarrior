using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public void OnMoveInput(InputAction.CallbackContext context)
   {
        if(context.control.name.Contains("Stick"))
        {
          
            Vector2 joystickInput = context.ReadValue<Vector2>();
            float angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;

         
            if (angle < 0)
            {
                angle += 360f;
            }
            float distance = joystickInput.magnitude;
            Debug.Log("Joystick Angle: " + angle+"   distance "+distance);
        } else
        {
            Debug.Log("Keyboard Input");
        }
   }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Started");
        }

        if (context.performed) 
        {
            Debug.Log("performed");
        }

        if (context.canceled)
        {
            Debug.Log("canceled");
        }
    }

    public void OnLockAndFireInput(InputAction.CallbackContext context)
    {
  
        Vector2 joystickInput = context.ReadValue<Vector2>();
        float angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;


        if (angle < 0)
        {
            angle += 360f;
        }
        float distance = joystickInput.magnitude;
        Debug.Log("Joystick Angle: " + angle + "   distance " + distance);
        
    }
}
