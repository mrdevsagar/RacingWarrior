using System.Collections.Generic;
using UnityEngine;

public class VehicleCollisionHandler : MonoBehaviour
{
    
    public float rayDistance = 10f;  
    public LayerMask hitLayers;

    public Drive drive;



    void Update()
    {
        RaycastHit2D hitRight = HitRay(new Vector2(transform.position.x - 1.3f, transform.position.y), Vector2.right, Color.red);

        RaycastHit2D hit2 = HitRay(new Vector2(transform.position.x + 1.3f, transform.position.y), -Vector2.right, Color.green);

        Slope slopeRight =  hitRight.collider.gameObject.GetComponent<Slope>();

        if (slopeRight != null)
        {
            Debug.LogWarning("yooo"+ slopeRight.InSlopeObjects[0].name);
            drive.DisableCollisionForVehicle(slopeRight.InSlopeObjects[0].GetComponent<Collider2D>());
        }


    }

    public RaycastHit2D HitRay(Vector2 origin, Vector2 direction, Color color)
    {

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, hitLayers);

        Debug.DrawRay(origin, direction * rayDistance, color);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
        }

        return hit;
    }

}
