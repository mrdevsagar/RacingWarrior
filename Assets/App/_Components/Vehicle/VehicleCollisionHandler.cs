using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class VehicleCollisionHandler : MonoBehaviour
{
    
    public float rayDistance = 2f;  
    public LayerMask hitLayers;

    public Drive drive;

    private Collider2D previousHitRight = null;

    private Collider2D previousHitLeft = null;

    private Rigidbody2D RB;

    public List<GameObject> Slopes;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Slopes = new List<GameObject>();
    }

    void Update()
    {

        /*SlopeLeftInside();
        SlopeRightInside();*/

    }


    public void OnNewSlopeStayed(GameObject slopeObj)
    {
       

        RaycastHit2D hitRight = HitRay(new Vector2(transform.position.x - 1.3f, transform.position.y), Vector2.right, Color.blue);
        Collider2D currentHitRight = null;
        if (hitRight.collider != null && hitRight.collider.gameObject.CompareTag("SlopeLeft"))
        {
            currentHitRight = hitRight.collider;
        }

        // OnEnter: If we just hit a new object
        if (currentHitRight != null  && RB.velocityX > 0)
        {
            Slope slopeRight = currentHitRight.gameObject.GetComponent<Slope>();
            EnableSlopeTop(slopeRight, false);

            previousHitRight = currentHitRight;
        } else
        {
            if (Slopes != null && !Slopes.Contains(slopeObj))
            {
                Slopes.Add(slopeObj);
            }
            if (Slopes != null && slopeObj.CompareTag("SlopeLeft"))
            {
                Slope slope = slopeObj.GetComponent<Slope>();
                EnableSlopeTop(slope);
            }
        }

        RaycastHit2D hitLeft = HitRay(new Vector2(transform.position.x + 1.3f, transform.position.y), -Vector2.right, Color.cyan);

        Collider2D currentHitLeft =  null;

        if  (hitLeft.collider != null && hitLeft.collider.gameObject.CompareTag("SlopeRight"))
        {
            currentHitLeft = hitLeft.collider;
        }

        if (currentHitLeft != null  && RB.velocityX < 0)
        {
                Slope slopeLeft = currentHitLeft.gameObject.GetComponent<Slope>();
                EnableSlopeTop(slopeLeft, false);

            previousHitLeft = currentHitLeft;
        }
        else
        {
            if (Slopes != null && !Slopes.Contains(slopeObj))
            {
                Slopes.Add(slopeObj);
            }
            if (Slopes != null && slopeObj.CompareTag("SlopeRight"))
            {
                Slope slope = slopeObj.GetComponent<Slope>();
                EnableSlopeTop(slope);
            }
        }

    }

    public void OnSlopeExits(GameObject slopeObj)
    {
        if (Slopes != null)
        {
            Slopes.Remove(slopeObj);

            Slope slope = slopeObj.GetComponent<Slope>();
            EnableSlopeAll(slope);
        }
    }
   

    

    public void SlopeLeftInside()
    {
        RaycastHit2D hitRight = HitRay(new Vector2(transform.position.x - 1.3f, transform.position.y), Vector2.right, Color.blue);

        // Check if we have a hit
        Collider2D currentHitRight = hitRight.collider;

        if (currentHitRight != null && currentHitRight.gameObject.CompareTag("SlopeRight"))
        {
            return;
        }

        // OnEnter: If we just hit a new object
        if (currentHitRight != null && currentHitRight != previousHitRight && RB.velocityX > 0)
        {
            Slope slopeRight = currentHitRight.gameObject.GetComponent<Slope>();
            EnableSlopeTop(slopeRight, false);
        }

        // OnExit: If we no longer hit the previous object
        if (previousHitRight != null && currentHitRight != previousHitRight)
        {
            Debug.Log("OnExit: " + previousHitRight.name);
        }

        // Update the previous hit
        previousHitRight = currentHitRight;
    }

    public void SlopeRightInside()
    {

        RaycastHit2D hitLeft = HitRay(new Vector2(transform.position.x + 1.3f, transform.position.y), -Vector2.right, Color.cyan);

        Collider2D currentHitLeft = hitLeft.collider;

        if (currentHitLeft != null && currentHitLeft.gameObject.CompareTag("SlopeLeft"))
        {
            return;
        }
        else
        {

            // OnEnter: If we just hit a new object
            if (currentHitLeft != null && currentHitLeft != previousHitLeft && RB.velocityX < 0)
            {
                Slope slopeLeft = currentHitLeft.gameObject.GetComponent<Slope>();
                EnableSlopeTop(slopeLeft, false);
            }
        }


        // OnExit: If we no longer hit the previous object
        if (previousHitLeft != null && currentHitLeft != previousHitLeft)
        {
            Debug.Log("OnExit: " + previousHitLeft?.name);
        }

        // Update the previous hit
        previousHitLeft = currentHitLeft;


    }
    private void EnableSlopeTop(Slope  slope,bool isEnableTop = true)
    {
        if (slope != null)
        {
            if (isEnableTop)
            {
                foreach (var obj in slope.OnSlopeObjects)
                {
                    Collider2D col = obj.GetComponent<Collider2D>();

                    if (col != null)
                    {
                        drive.EnableCollisionForVehicle(col);
                    }
                }

                foreach (var obj in slope.InSlopeObjects)
                {   
                    Collider2D col = obj.GetComponent<Collider2D>();

                    if (col != null)
                    {
                        drive.DisableCollisionForVehicle(col);
                    }
                }

            } else
            {
                foreach (var obj in slope.OnSlopeObjects)
                {
                    Collider2D col = obj.GetComponent<Collider2D>();

                    if (col != null)
                    {
                        drive.DisableCollisionForVehicle(col);
                    }
                }

                foreach (var obj in slope.InSlopeObjects)
                {
                    Collider2D col = obj.GetComponent<Collider2D>();

                    if (col != null)
                    {
                        drive.EnableCollisionForVehicle(col);
                    }
                }
            }
            
        }
    }
    
    private void EnableSlopeAll(Slope slope)
    {
        if (slope != null)
        {
            foreach (var obj in slope.OnSlopeObjects)
            {
                Collider2D col = obj.GetComponent<Collider2D>();

                if (col != null)
                {
                    drive.EnableCollisionForVehicle(col);
                }
            }

            foreach (var obj in slope.InSlopeObjects)
            {
                Collider2D col = obj.GetComponent<Collider2D>();

                if (col != null)
                {
                    drive.DisableCollisionForVehicle(col);
                }
            }
        }
    }


    private RaycastHit2D HitRay(Vector2 origin, Vector2 direction, Color color)
    {

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, hitLayers);

        Debug.DrawRay(origin, direction * rayDistance, color);


        return hit;
    }

}
