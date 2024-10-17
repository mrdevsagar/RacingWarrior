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
        if (drive.DisabledVehicleObjects != null && !drive.DisabledVehicleObjects.Contains(collision.gameObject))
        {
            drive.DisabledVehicleObjects.Add(collision.gameObject);
            drive.DisableCollisionForVehicle(collision);
        }
    }
}