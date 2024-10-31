using UnityEngine;
public class CapsuleRotation : MonoBehaviour
{
    public Transform childObject; // Assign your child object in the inspector
    public float capsuleWidth = 10f; // Distance from the parent to the capsule

    void Start()
    {
        // Set the initial position of the child object based on the capsule width
        UpdateChildPosition();
    }

    void Update()
    {
        // Get the parent's rotation
        Quaternion parentRotation = transform.rotation;

        // Set the child's rotation to match the parent's rotation
        childObject.rotation = parentRotation;

        // Update the child's position based on the capsule width
        UpdateChildPosition();
    }

    void UpdateChildPosition()
    {
        // Calculate the position offset based on the width of the capsule
        Vector3 offset = transform.up * (capsuleWidth / 2); // Adjust based on your capsule's orientation

        // Set the child's position based on the parent's position and the calculated offset
        childObject.position = transform.position + offset;
    }
}
