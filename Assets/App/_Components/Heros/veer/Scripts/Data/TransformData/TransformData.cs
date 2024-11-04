using UnityEngine;

[CreateAssetMenu(fileName = "TransformData", menuName = "ScriptableObjects/TransformData")]
public class TransformData : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public void SaveTransform(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    public void LoadTransform(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}
