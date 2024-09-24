using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput {  get; private set; }
    public float LookInput { get; private set; }
    public float LookDragDistance { get; private set; }
    public bool IsFiring { get; private set; }

    public TextMeshProUGUI textBox;

    private PlayerInputActions playerInput; // Replace with your input action class name


  
    private Vector2 moveInput;

    private string controlName;

    private void Awake()
    {
        // Initialize the input actions
        playerInput = new PlayerInputActions();

        // Bind the input action callbacks
        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMovePerformed;
        LookInput = float.NaN;
    }

    private void OnEnable()
    {
        // Enable the input action map
        playerInput.Player.Enable(); // Replace "Player" with your action map name
    }

    private void OnDisable()
    {
        // Disable the input action map
        playerInput.Player.Disable();
    }

    // Callback for the Move input action
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        var control = context.control;
        controlName = control.name;  // Capture the name of the control
    }
    private void Update()
    {
        GetMoveInput();
        GetLockAndFireInput();
    }
    public void GetMoveInput()
   {
        Vector2 rawInput = playerInput.Player.Move.ReadValue<Vector2>();

        // Optionally debug the control name if needed
        /*if (!string.IsNullOrEmpty(controlName))
        {
            Debug.Log("Control Name: " + controlName);
        }*/

        if (!string.IsNullOrEmpty(controlName) && controlName.Contains("Stick"))
        {
          
            Vector2 joystickInput = rawInput;
            float angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;

         
            if (angle < 0)
            {
                angle += 360f;
            }
            float distance = joystickInput.magnitude;

            if ((angle >= 0 && angle < 90) || (angle >= 350 && angle < 360))
            {
                MoveInput = new Vector2(1, 1);
            } else if (angle >= 90 && angle < 190)
            {
                MoveInput = new Vector2(-1,1);
            } else if (angle >= 190 && angle < 270)
            {
                MoveInput = new Vector2(-1, -1);
            } else if (angle >= 270 && angle < 350)
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
                else
                {
                    MoveInput = Vector2.zero;
                }
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

    public void GetLockAndFireInput()
    {
        Vector2 joystickInput = playerInput.Player.Look.ReadValue<Vector2>();

        float angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;


        if (angle < 0)
        {
            angle += 360f;
        }

        float distance = joystickInput.magnitude;
        
        if (distance > 0.05f)
        {
            LookDragDistance = distance;
            LookInput = angle;
        } else
        {
            LookDragDistance = float.NaN;
            LookInput = float.NaN;
        }

        if (distance > 0.98f)
        {
            IsFiring = true;
        } else
        {
            IsFiring = false;
        }

        textBox.text = angle.ToString();
    }
}
