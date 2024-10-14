using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalVehicleCollider : MonoBehaviour
{
    public Drive drive;

    private void Awake()
    {
        /*drive = transform.parent.GetComponent<Drive>();*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (drive.DisabledSlopeObjects != null && !drive.DisabledSlopeObjects.Contains(collision.gameObject))
        {
            drive.DisabledSlopeObjects.Add(collision.gameObject);
            drive.DisableCollisionForVehicle(collision);
        }
    }
}