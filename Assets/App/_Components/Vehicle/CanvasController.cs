

using System;
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

    // Define the delegate type for the event
    public delegate void PlayerControlsEvent(bool isPlayerControls);

    // Define the event
    public static event PlayerControlsEvent OnPlayerControls;

    // Static method to invoke the event


    // Define the delegate type for the event
    public delegate void PlayerNearVehicleEvent(bool isPlayerVehicle);
    // Define the event
    public static event PlayerNearVehicleEvent OnPlayerNearVehicle;


    // Define the delegate type for the event
    public delegate void PlayerJump();
    // Define the event
    public static event PlayerJump OnPlayerJump;


    [SerializeField]
    private GameObject PlayerControls;
    [SerializeField]
    private GameObject VehicleControls;

    [SerializeField]
    private GameObject GetInVehicleBtn_GO;

    private void Start()
    {
        PlayerControls.SetActive(true);
    }

    private void OnEnable()
    {
        OnPlayerControls += OnPlayerControlsChange;
        OnPlayerNearVehicle += OnPlayerNearVehicleTriggered;
    }

    private void OnDisable()
    {
        OnPlayerControls -= OnPlayerControlsChange;
        OnPlayerNearVehicle -= OnPlayerNearVehicleTriggered;
    }

    public void OnPlayerControlsChange(bool isPlayerControls)
    {
        if (isPlayerControls)
        {
            PlayerControls.SetActive(true);
            VehicleControls.SetActive(false);
        }
        else
        {
            PlayerControls.SetActive(false);
            VehicleControls.SetActive(true);
        }
    }

    public void OnPlayerNearVehicleTriggered(bool isPlayerVehicle)
    {
        if (isPlayerVehicle)
        {
            GetInVehicleBtn_GO.SetActive(true);
        }
        else
        {
            GetInVehicleBtn_GO.SetActive(false);
        }
    }




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

    public static void SwitchControls(bool isPlayerControls)
    {
        // Check if there are any subscribers to the event before invoking
        OnPlayerControls?.Invoke(isPlayerControls);
    }

    public static void IsPlayerNearVehicle(bool isPlayerVehicle)
    {
        OnPlayerNearVehicle?.Invoke(isPlayerVehicle);
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
