using Unity.VisualScripting;
using UnityEngine;

public class ArrowStick : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;
    private Vector3 scale;
    Collider2D col;
    private SpriteRenderer spriteRenderer;

    public bool IsFiredRight = false;
    public bool IsFiredLeft = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (IsFiredRight && !hasCollided)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (IsFiredLeft && !hasCollided)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

            // Negate the angle for left firing
            angle = angle + 180; // Rotate 180 degrees to face the left direction

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided)
        {
            rb.velocity = Vector3.zero;
            /*rb.freezeRotation = true;*/
            rb.isKinematic = true;
            Destroy(rb);
            Destroy(col);
            Debug.LogWarning(collision.gameObject.name);
            
            transform.parent = collision.transform;



            /*spriteRenderer.sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;*/
            // Make sure it stays at its current position and state
            hasCollided = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided)
        {
            rb.freezeRotation = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            /*Destroy(col);*/
           /* Destroy(rb);*/
            Debug.LogWarning(collision.gameObject.name);
            transform.parent = collision.transform;

            spriteRenderer.sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder -1;
            // Make sure it stays at its current position and state
            hasCollided = true;
        }
    }
}
