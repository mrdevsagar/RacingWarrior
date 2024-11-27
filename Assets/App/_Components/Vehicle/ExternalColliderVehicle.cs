// Ignore Spelling: Collider

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExternalColliderVehicle : MonoBehaviour
{
    public Drive drive;

    public GameObject CurrentRightSlope;
    public GameObject CurrentLeftSlope;

    public GameObject CurrentRightPlatform;
    public GameObject CurrentLeftPlatform;

    public bool isExternalCollider2;

    public float VehicleSpeed = 10f;


    private void Awake()
    {
        /*drive = transform.parent.GetComponent<Drive>();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlopeRight"))
        {
            if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(collision.gameObject))
            {
                CurrentRightSlope = collision.gameObject;
            }
            else
            {
                CurrentRightSlope = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeLeft"))
        {
            if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(collision.gameObject))
            {
                CurrentLeftSlope = collision.gameObject;
            }
            else
            {
                CurrentLeftSlope = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeRightPlatform"))
        {
            if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(collision.gameObject))
            {
                CurrentRightPlatform = collision.gameObject;
            }
            else
            {
                CurrentRightPlatform = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeLeftPlatform"))
        {
            if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(collision.gameObject))
            {
                CurrentLeftPlatform = collision.gameObject;
            }
            else
            {
                CurrentLeftPlatform = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (drive.DisabledVehicleObjects != null && drive.DisabledVehicleObjects.Contains(collision.gameObject))
        {
            drive.DisabledVehicleObjects.Remove(collision.gameObject);
            drive.EnableCollisionForVehicle(collision);
        }

        if (!(CurrentRightSlope == null) && CurrentRightSlope == collision.gameObject)
        {
            CurrentRightSlope = null;
        }

        if (!(CurrentLeftSlope == null) && CurrentLeftSlope == collision.gameObject)
        {
            CurrentLeftSlope = null;
        }

        if (!(CurrentRightPlatform == null) && CurrentRightPlatform == collision.gameObject)
        {
            CurrentRightPlatform = null;
        }

        if (!(CurrentLeftPlatform == null) && CurrentLeftPlatform == collision.gameObject)
        {
            CurrentLeftPlatform = null;
        }
    }


    private void Update()
    {

        if ((drive.VehicleRB.linearVelocity.x > VehicleSpeed && isExternalCollider2) || (drive.VehicleRB.linearVelocity.x < -VehicleSpeed && isExternalCollider2) || (drive.VehicleRB.linearVelocity.x <= VehicleSpeed && !isExternalCollider2) || (drive.VehicleRB.linearVelocity.x >= -VehicleSpeed && !isExternalCollider2))
        {
            if (CurrentRightSlope != null && drive.input.MoveInput.x > 0 && drive.input.MoveInput.y < 0)
            {

                if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(CurrentRightSlope))
                {
                    drive.DisableCollisionForVehicle(CurrentRightSlope.GetComponent<Collider2D>());
                    drive.DisabledVehicleObjects.Add(CurrentRightSlope);
                }
            }

            if (CurrentLeftSlope != null && drive.input.MoveInput.x < 0 && drive.input.MoveInput.y < 0)
            {
                if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(CurrentLeftSlope))
                {
                    drive.DisableCollisionForVehicle(CurrentLeftSlope.GetComponent<Collider2D>());
                    drive.DisabledVehicleObjects.Add(CurrentLeftSlope);
                }
            }

            if (CurrentRightPlatform != null && drive.input.MoveInput.x < 0 && drive.input.MoveInput.y < 0)
            {

                if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(CurrentRightPlatform))
                {
                    drive.DisableCollisionForVehicle(CurrentRightPlatform.GetComponent<Collider2D>());
                    drive.DisabledVehicleObjects.Add(CurrentRightPlatform);
                }
            }

            if (CurrentLeftPlatform != null && drive.input.MoveInput.x > 0 && drive.input.MoveInput.y < 0)
            {
                if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(CurrentLeftPlatform))
                {
                    drive.DisableCollisionForVehicle(CurrentLeftPlatform.GetComponent<Collider2D>());
                    drive.DisabledVehicleObjects.Add(CurrentLeftPlatform);
                }
            }

        }
    }
  
}