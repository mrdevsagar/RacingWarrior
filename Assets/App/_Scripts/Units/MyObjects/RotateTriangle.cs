using UnityEngine;

public class RotateTriangle : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the triangle around its Z axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
