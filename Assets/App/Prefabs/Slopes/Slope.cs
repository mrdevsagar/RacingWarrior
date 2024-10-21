using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Slope : MonoBehaviour
{

    // Platform effector setup
    public PlatformEffector2D platformEffector; // Drag your effector here
    public float allowedAngle = 180f; // Set the angle range for collision detection

    [SerializeField]
    public List<GameObject> OnSlopeObjects;

    [SerializeField]
    public List<GameObject> InSlopeObjects;

    [SerializeField]
    public Slope ChildSlope =  null;

    [SerializeField]
    public List<GameObject> IntersectedParentSlopeObjects;

    public UnityAction<GameObject> callbackEnter;
    public UnityAction<GameObject> callbackExit;

    [SerializeField]
    private bool isCholdSlope;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }
    private void Start()
    {
        OnSlopeObjects = new List<GameObject>();
        InSlopeObjects = new List<GameObject>();
        IntersectedParentSlopeObjects = new List<GameObject>();


        if (ChildSlope != null && !isCholdSlope)
        {
            ChildSlope.callbackEnter = OnChildInternalCollideEnter;
            ChildSlope.callbackExit = OnChildInternalCollideExit;
        }
        

    }
    public float topAngleThreshold = 180f;

    private void OnCollisionEnter2D(Collision2D collision)
    {


        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }

            if (contact.normalImpulse > 0)
            {
                if (OnSlopeObjects != null)
                {
                    if (ChildSlope == null)
                    {
                        if (!OnSlopeObjects.Contains(collision.gameObject))
                        {
                            OnSlopeObjects.Add(collision.gameObject);
                        }
                    }
                    else if (!ChildSlope.InSlopeObjects.Contains(collision.gameObject))
                    {
                        OnSlopeObjects.Add(collision.gameObject);
                    }
                    else if (ChildSlope.InSlopeObjects.Contains(collision.gameObject))
                    {
                        IntersectedParentSlopeObjects.Add(collision.gameObject);
                    }
                }

            }
            else
            {
                /*Debug.Log("Passing through the slope or colliding from below");*/
            }
        }
    }

    public void OnChildInternalCollideEnter(GameObject gameObject)
    {
        if (OnSlopeObjects.Contains(gameObject))
        {
            OnSlopeObjects.Remove(gameObject);
        }
    }

    public void OnChildInternalCollideExit(GameObject gameObject)
    {
        if (IntersectedParentSlopeObjects.Contains(gameObject))
        {
            OnSlopeObjects.Add(gameObject);
            IntersectedParentSlopeObjects.Remove(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (OnSlopeObjects != null)
        {
            OnSlopeObjects.Remove(collision.gameObject);
            IntersectedParentSlopeObjects.Remove(collision.gameObject);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (InSlopeObjects != null && !InSlopeObjects.Contains(collision.gameObject) && collision.gameObject.tag == "EnemyInternal")
        {

            InSlopeObjects.Add(collision.gameObject.transform.parent.gameObject);


            if (ChildSlope == null && isCholdSlope)
            {
                
                callbackEnter?.Invoke(collision.gameObject.transform.parent.gameObject);
            }
        }

        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InSlopeObjects != null && collision.gameObject.tag == "EnemyInternal")
        {
            InSlopeObjects.Remove(collision.gameObject.transform.parent.gameObject);

            if (ChildSlope == null && isCholdSlope)
            {
                callbackExit?.Invoke(collision.gameObject.transform.parent.gameObject);
            }
        }

        
    }    

    
}
