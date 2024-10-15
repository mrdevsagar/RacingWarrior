using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExternalColliderVehicle : MonoBehaviour
{
    public Drive drive;

    public GameObject CurruntRightSlope;
    public GameObject CurruntLeftSlope;
    public bool isExternalCollider2;


    private void Awake()
    {
        /*drive = transform.parent.GetComponent<Drive>();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlopeRight"))
        {
            if (drive.DisabledSlopeObjects != null && !drive.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurruntRightSlope = collision.gameObject;
            }
            else
            {
                CurruntRightSlope = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeLeft"))
        {
            if (drive.DisabledSlopeObjects != null && !drive.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurruntLeftSlope = collision.gameObject;
            }
            else
            {
                CurruntLeftSlope = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (drive.DisabledSlopeObjects != null && drive.DisabledSlopeObjects.Contains(collision.gameObject))
        {
            drive.DisabledSlopeObjects.Remove(collision.gameObject);
            drive.EnableCollisionForVehicle(collision);
        }

        if (!(CurruntRightSlope == null) && CurruntRightSlope == collision.gameObject)
        {
            CurruntRightSlope = null;
        }

        if (!(CurruntLeftSlope == null) && CurruntLeftSlope == collision.gameObject)
        {
            CurruntLeftSlope = null;
        }
    }


    private void Update()
    {

        if ((drive.VehicleRB.velocity.x > 15 && isExternalCollider2) || (drive.VehicleRB.velocity.x < -15 && isExternalCollider2) || (drive.VehicleRB.velocity.x <= 15 && !isExternalCollider2) || (drive.VehicleRB.velocity.x >= -15 && !isExternalCollider2))
        {
            if (CurruntRightSlope != null && drive.input.MoveInput.x > 0 && drive.input.MoveInput.y < 0)
            {

                if (drive.DisabledSlopeObjects != null && !drive.DisabledSlopeObjects.Contains(CurruntRightSlope))
                {
                    drive.DisableCollisionForVehicle(CurruntRightSlope.GetComponent<Collider2D>());
                    drive.DisabledSlopeObjects.Add(CurruntRightSlope);
                }
            }

            if (CurruntLeftSlope != null && drive.input.MoveInput.x < 0 && drive.input.MoveInput.y < 0)
            {
                if (drive.DisabledSlopeObjects != null && !drive.DisabledSlopeObjects.Contains(CurruntLeftSlope))
                {
                    drive.DisableCollisionForVehicle(CurruntLeftSlope.GetComponent<Collider2D>());
                    drive.DisabledSlopeObjects.Add(CurruntLeftSlope);
                }
            }

        }
    }

}