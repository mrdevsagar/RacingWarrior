using UnityEngine;

[System.Serializable]
public class JointData 
{
    [SerializeField]
    [Tooltip("Assign the HingeJoint2D component here.")]
    public HingeJoint2D hingeJoint;

    [Tooltip("First Vector2 value.")]
    public Vector2 vectorA;

    [Tooltip("Second Vector2 value.")]
    public Vector2 vectorB;

}
