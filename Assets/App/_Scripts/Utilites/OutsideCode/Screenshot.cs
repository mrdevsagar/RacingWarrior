using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Screenshot : MonoBehaviour
{
    private InputAction _touchPressAction;
    private PlayerInput playerInput;
    /*private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _touchPressAction = playerInput.actions["TouchPress"];
        DontDestroyOnLoad(gameObject);

    }
    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
    }*/

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Debug.Log("tuck screen shot");
        ScreenCapture.CaptureScreenshot("screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", 4);
    }
}