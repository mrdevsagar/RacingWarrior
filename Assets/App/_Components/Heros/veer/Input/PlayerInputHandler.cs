using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput {  get; private set; }

    public TextMeshProUGUI textBox;
    public void OnMoveInput(InputAction.CallbackContext context)
   {
        Vector2 rawInput = context.ReadValue<Vector2>();

        textBox.text = rawInput.ToString();
        if (context.control.name.Contains("Stick"))
        {
          
            Vector2 joystickInput = rawInput;
            float angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;

         
            if (angle < 0)
            {
                angle += 360f;
            }
            float distance = joystickInput.magnitude;

            if ((angle >= 0 && angle < 90) || (angle >= 340 && angle < 360))
            {
                MoveInput = new Vector2(1, 1);
            } else if (angle >= 90 && angle < 200)
            {
                MoveInput = new Vector2(-1,1);
            } else if (angle >= 200 && angle < 270)
            {
                MoveInput = new Vector2(-1, -1);
            } else if (angle >= 270 && angle < 340)
            {
                MoveInput = new Vector2(1, -1);
            }

            if (distance < 0.2)
            {
                MoveInput = Vector2.zero;
            }

        } else
        {
            if (rawInput.x == 0 || rawInput.x == 1 || rawInput.x == -1)
            {
                if (rawInput.y == 1)
                {
                    MoveInput = new Vector2(1, 1);
                }
                else if (rawInput.x == 1)
                {
                    MoveInput = new Vector2(-1, 1);
                }
                else if (rawInput.x == -1)
                {
                    MoveInput = new Vector2(-1, -1);
                }
                else if (rawInput.y == -1)
                {
                    MoveInput = new Vector2(1, -1);
                }
                /*else
                {
                    MoveInput = Vector2.zero;
                }*/
            }
               
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            /*Debug.Log("Started");*/
        }

        if (context.performed) 
        {
            /*Debug.Log("performed");*/
        }

        if (context.canceled)
        {
           /* Debug.Log("canceled");*/
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
        /*Debug.Log("Joystick Angle: " + angle + "   distance " + distance);*/
        
    }
}
