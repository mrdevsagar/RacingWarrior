using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{

    // Platform effector setup
    public PlatformEffector2D platformEffector; // Drag your effector here
    public float allowedAngle = 180f; // Set the angle range for collision detection

    [SerializeField]
    public List<GameObject> OnSlopeObjects;

    [SerializeField]
    public List<GameObject> InSlopeObjects;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }
    private void Start()
    {
        OnSlopeObjects = new List<GameObject>();
        InSlopeObjects = new List<GameObject>();
    }
    public float topAngleThreshold = 180f;

    private void OnCollisionEnter2D(Collision2D collision)
    {


        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.Log(contact.normalImpulse+ collision.gameObject.name);
            // Check if the normal vector indicates the object is on top of the slope
            if (contact.normalImpulse > 0)
            {
                Debug.Log("On top of the slope");

                if (OnSlopeObjects != null && !OnSlopeObjects.Contains(collision.gameObject))
                {
                   /* if (collision.gameObject.tag != "Player")*/
                    OnSlopeObjects.Add(collision.gameObject);
                }
            }
            else
            {
                Debug.Log("Passing through the slope or colliding from below");
            }
        }
    }

   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (OnSlopeObjects != null)
        {
            OnSlopeObjects.Remove(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (InSlopeObjects != null && !InSlopeObjects.Contains(collision.gameObject) && collision.gameObject.tag == "EnemyInternal")
        {
            InSlopeObjects.Add(collision.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InSlopeObjects != null && collision.gameObject.tag == "EnemyInternal")
        {
            InSlopeObjects.Remove(collision.gameObject.transform.parent.gameObject);
        }

        
    }
}
