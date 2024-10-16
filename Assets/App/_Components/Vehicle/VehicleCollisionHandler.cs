using System.Collections.Generic;
using UnityEngine;

public class VehicleCollisionHandler : MonoBehaviour
{
    
    public float rayDistance = 2f;  
    public LayerMask hitLayers;

    public Drive drive;

    private Collider2D previousHitRight = null;

    private Rigidbody2D RB;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        RaycastHit2D hitRight = HitRay(new Vector2(transform.position.x, transform.position.y), Vector2.right, Color.red);


        // Check if we have a hit
        Collider2D currentHitRight = hitRight.collider;

        // OnEnter: If we just hit a new object
        if (currentHitRight != null && currentHitRight != previousHitRight && RB.velocityX> 0)
        {
            Slope slopeRight = currentHitRight.gameObject.GetComponent<Slope>();
            if (slopeRight != null)
            {
                foreach (var obj in slopeRight.OnSlopeObjects)
                {
                    Debug.LogWarning("Object name: " + obj.name);
                    Collider2D col = obj.GetComponent<Collider2D>();

                    if (col != null)
                    {
                       /* drive.DisableCollisionForVehicle(col);*/
                    }
                }
            }
        }

        // OnExit: If we no longer hit the previous object
        if (previousHitRight != null && currentHitRight != previousHitRight)
        {
            /*Debug.Log("OnExit: " + previousHitRight.name);*/
        }

        // Update the previous hit
        previousHitRight = currentHitRight;


    }


    /* RaycastHit2D hit2 = HitRay(new Vector2(transform.position.x + 1.3f, transform.position.y), -Vector2.right, Color.green);*/
    public RaycastHit2D HitRay(Vector2 origin, Vector2 direction, Color color)
    {

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, hitLayers);

        Debug.DrawRay(origin, direction * rayDistance, color);

     
        return hit;
    }

}
