using UnityEngine;
using UnityEngine.InputSystem;
public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction _touchPositionAction;

    private InputAction _touchPressAction;

    private Camera mainCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _touchPositionAction = playerInput.actions["TouchPosition"];
        _touchPressAction = playerInput.actions["TouchPress"];
       
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector2 value = _touchPositionAction.ReadValue<Vector2>();
        PressedPostion(value);
    }

    private void PressedPostion(Vector2 touchPosition)
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Touched " + hit.collider.gameObject.name);

            // Perform actions based on which object was touched
            if (hit.collider.gameObject.name == "Sprite1")
            {
                Debug.Log("Sprite1 was touched!");
            }
            else if (hit.collider.gameObject.name == "Sprite2")
            {
                Debug.Log("Sprite2 was touched!");
            }
        }
    }
}
