

using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPressHandler : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction pointerAction;

    private void OnEnable()
    {
        pointerAction = inputActions.FindActionMap("Player").FindAction("Pointer");
        pointerAction.Enable();
        pointerAction.performed += OnPointerPerformed;
    }

    private void OnDisable()
    {
        pointerAction.performed -= OnPointerPerformed;
        pointerAction.Disable();
    }

    private void OnPointerPerformed(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Debug.Log("Square object pressed!");
            // Call your custom function here
            CustomFunction();
        }
    }

    // Your custom function
    private void CustomFunction()
    {
        // Add the functionality you want to trigger on press
        Debug.Log("Custom function called!");
    }
}
