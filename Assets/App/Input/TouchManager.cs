using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction _touchPositionAction;

    private InputAction _touchPressAction;

    private Camera mainCamera;

    [SerializeField] private SceneField ShopeScene;

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
        if (mainCamera == null) {

            Debug.Log("camera not found");
            return;
        }
        RaycastHit2D hit;

        if(mainCamera.orthographic )
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(touchPosition);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        } else
        {
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -mainCamera.transform.position.z));
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        }

        if (hit.collider != null)
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.tag);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                MyLoadSceneAsync.Instance.Load(ShopeScene);
            }
            else if (hit.collider.gameObject.CompareTag("Vehicle"))
            {
                MyLoadSceneAsync.Instance.Load(ShopeScene);
            }

        }
        else
        {
            Debug.Log("No object hit");
        }

    }
}
