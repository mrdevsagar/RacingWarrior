using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraSwitcher : MonoBehaviour
{
    public static event UnityAction<GameObject> OnSwitchToPlayer;
    public static event UnityAction<GameObject> OnSwitchToVehicle;

    public GameObject playerCamera;
    public GameObject vehicleCamera;

    private void OnEnable()
    {
        OnSwitchToPlayer += SwitchToPlayer;
        OnSwitchToVehicle += SwitchToVehicle;
    }

    private void OnDisable()
    {
        OnSwitchToPlayer -= SwitchToPlayer;
        OnSwitchToVehicle -= SwitchToVehicle;
    }

    // Public methods to invoke the events from other scripts
    public static void TriggerSwitchToPlayer(GameObject player)
    {
        OnSwitchToPlayer?.Invoke(player);
    }

    public static void TriggerSwitchToVehicle(GameObject vehicle)
    {
        OnSwitchToVehicle?.Invoke(vehicle);
    }
    private void SwitchToPlayer(GameObject player)
    {
        if (player != null)
        {
            playerCamera.SetActive(true); 
            vehicleCamera.SetActive(false);
        }
    }

    private void SwitchToVehicle(GameObject vehicle)
    {
        if (vehicle != null)
        {
            vehicleCamera.GetComponent<CinemachineCamera>().Follow = vehicle.transform;
            vehicleCamera.SetActive(true);
            playerCamera.SetActive(false);
            
        }
    }
}
