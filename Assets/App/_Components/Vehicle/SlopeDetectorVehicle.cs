using UnityEngine;

public class SlopeDetectorVehicle : MonoBehaviour
{
    [SerializeField]
    private VehicleCollisionHandler CollisionHandler;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "SlopeRight":
                CollisionHandler.LeftSlope = collision.gameObject;
                break;

            case "SlopeLeft":
                CollisionHandler.RightSlope = collision.gameObject;
                break;

            case "SlopeRightPlatform":
                CollisionHandler.RightSlopePlatform = collision.gameObject;
                break;

            case "SlopeLeftPlatform":
                CollisionHandler.LeftSlopePlatform = collision.gameObject;
                break;

            default: break;
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "SlopeRight":
                if (CollisionHandler.LeftSlope == collision.gameObject)
                {
                    CollisionHandler.LeftSlope = null;
                }
                break;

            case "SlopeLeft":
                if (CollisionHandler.RightSlope == collision.gameObject)
                {
                    CollisionHandler.RightSlope = null;
                }
                break;

            case "SlopeRightPlatform":
                if (CollisionHandler.RightSlopePlatform == collision.gameObject)
                {
                    CollisionHandler.RightSlopePlatform = null;
                }
                break;

            case "SlopeLeftPlatform":
                if (CollisionHandler.LeftSlopePlatform == collision.gameObject)
                {
                    CollisionHandler.LeftSlopePlatform = null;
                }
                break;

            default: break;

        }
    }

}
