using UnityEngine;

public class SlopeDetectorVehicle : MonoBehaviour
{
    public VehicleCollisionHandler collisionHandler;
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionHandler.OnSlopeExits(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collisionHandler.OnNewSlopeStayed(collision.gameObject);
    }
}
