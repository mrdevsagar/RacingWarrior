using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class VehicleCollisionHandler : MonoBehaviour
{
    
    public float rayDistance = 2f;  
    public LayerMask hitLayers;

    public LayerMask hitLayersTop;


    public Drive drive;



    private Rigidbody2D RB;

    public List<GameObject> Slopes;

    public GameObject LeftSlope = null;
    public GameObject LeftSlopePlatform = null;
    public GameObject RightSlope = null;
    public GameObject RightSlopePlatform = null;
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Slopes = new List<GameObject>();
    }

    void Update()
    {
        OnNewSlopeStayed();
    }

    

    public void OnNewSlopeStayed()
    {
       

        RaycastHit2D hitRight = HitRay(new Vector2(transform.position.x - 1.3f, transform.position.y), Vector2.right, Color.blue, hitLayers);
        Collider2D currentHitRight = null;
        if (hitRight.collider != null && hitRight.collider.gameObject.CompareTag("SlopeLeft"))
        {
            currentHitRight = hitRight.collider;
        }

        // OnEnter: If we just hit a new object
        /* if (currentHitRight != null  && RB.velocityX > 0)*/
        if (currentHitRight != null)
        {
            Slope slopeRight = currentHitRight.gameObject.GetComponent<Slope>();
            EnableSlopeTop(slopeRight, false);

      
        } else
        {
            
            if (LeftSlope != null)
            {
                /*Slope slope = slopeObj.GetComponent<Slope>();
                EnableSlopeTop(slope);*/

                if (drive.DisabledVehicleObjects.Contains(LeftSlope))
                {
                    Slope slope = LeftSlope.GetComponent<Slope>();
                    EnableSlopeTop(slope, false);
                }
                else
                {
                    Slope slope = LeftSlope.GetComponent<Slope>();
                    EnableSlopeTop(slope);
                }
            }
        }

        RaycastHit2D hitLeft = HitRay(new Vector2(transform.position.x + 1.3f, transform.position.y), -Vector2.right, Color.cyan, hitLayers);

        Collider2D currentHitLeft =  null;

        if  (hitLeft.collider != null && hitLeft.collider.gameObject.CompareTag("SlopeRight"))
        {
            currentHitLeft = hitLeft.collider;
        }

        /*if (currentHitLeft != null  && RB.velocityX < 0)*/
        if (currentHitLeft != null)
        {
                Slope slopeLeft = currentHitLeft.gameObject.GetComponent<Slope>();
                EnableSlopeTop(slopeLeft, false);


        }
        else
        {
          
            if (RightSlope != null)
            {
                if (drive.DisabledVehicleObjects.Contains(RightSlope))
                {
                    Slope slope = RightSlope.GetComponent<Slope>();
                    EnableSlopeTop(slope, false);
                }
                else
                {
                    Slope slope = RightSlope.GetComponent<Slope>();
                    EnableSlopeTop(slope);
                }
            }
        }

        RaycastHit2D hitTop = HitRay(new Vector2(transform.position.x  , transform.position.y - 0.8f ), Vector2.up, Color.yellow, hitLayersTop);
        Collider2D currentHitTopRight = null;
        Collider2D currentHitTopLeft = null;

        if (hitTop.collider != null)
        {
             if (hitTop.collider.gameObject.CompareTag("SlopeRightPlatform"))
             {
                 currentHitTopRight = hitTop.collider;
             } else if (hitTop.collider.gameObject.CompareTag("SlopeLeftPlatform"))
             {
                currentHitTopLeft = hitTop.collider;
             }
        }



        // OnEnter: If we just hit a new object
        if (currentHitTopRight != null )
        {
            Slope slopeTopRight = currentHitTopRight.gameObject.GetComponent<Slope>();
            EnableSlopeTop(slopeTopRight, false);
        }
        else
        {
            if (RightSlopePlatform != null)
            {
                if (drive.DisabledVehicleObjects.Contains(RightSlopePlatform))
                {
                    Slope slope = RightSlopePlatform.GetComponent<Slope>();
                    EnableSlopeTop(slope, false);
                }
                else
                {
                    Slope slope = RightSlopePlatform.GetComponent<Slope>();
                    EnableSlopeTop(slope);
                }
            }
        }

        if (currentHitTopLeft != null && RB.linearVelocityX < 0)
        {
            Slope slopeTopLeft = currentHitTopLeft.gameObject.GetComponent<Slope>();
            EnableSlopeTop(slopeTopLeft, false);
        }
        else
        {
            
            if (LeftSlopePlatform != null )
            {
                if (drive.DisabledVehicleObjects.Contains(LeftSlopePlatform))
                {
                    Slope slope = LeftSlopePlatform.GetComponent<Slope>();
                    EnableSlopeTop(slope, false);
                }
                else
                {
                    Slope slope = LeftSlopePlatform.GetComponent<Slope>();
                    EnableSlopeTop(slope);
                }
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


    private RaycastHit2D HitRay(Vector2 origin, Vector2 direction, Color color, LayerMask hitLayers)
    {

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, hitLayers);

        Debug.DrawRay(origin, direction * rayDistance, color);


        return hit;
    }

}
