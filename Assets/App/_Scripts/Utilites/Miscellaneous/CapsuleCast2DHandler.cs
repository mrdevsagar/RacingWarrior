using UnityEngine;

public class CapsuleCast2DHandler
{
    // Capsule cast properties
    public Vector2 pointA;          // First point of the capsule's line segment
    public Vector2 pointB;          // Second point of the capsule's line segment
    public float capsuleRadius;     // Radius of the capsule
    public LayerMask layerMask;     // Layers to check for collisions

    // Constructor to initialize the capsule properties
    public CapsuleCast2DHandler(float radius, LayerMask mask)
    {
        capsuleRadius = radius;
        layerMask = mask;
    }

    // Combined method to update the capsule points and perform the capsule cast
    public RaycastHit2D UpdateAndPerformCapsuleCast(Transform transform, float capsuleHeight, Vector2 direction, float distance)
    {
        // Update capsule points based on the Transform position
        Vector2 position = transform.position;
        pointA = new Vector2(position.x, position.y + capsuleHeight / 2); // Top point
        pointB = new Vector2(position.x, position.y - capsuleHeight / 2); // Bottom point

        // Perform the capsule cast and return the result
        return Physics2D.CapsuleCast(pointA, pointB, CapsuleDirection2D.Vertical, 0f, direction, distance, layerMask);
    }

    // Method to visualize the capsule in the scene (for debugging)
    public void DrawCapsuleGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, capsuleRadius);
        Gizmos.DrawWireSphere(pointB, capsuleRadius);
        Gizmos.DrawLine(pointA, pointB);
    }
}
