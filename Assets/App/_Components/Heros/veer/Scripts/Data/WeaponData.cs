using UnityEngine;


[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Base Data")]
public class WeaponData : ScriptableObject
{
    public float ArrowSpeed = 10f;

    [Header("Rifle")]
    public TransformData ParentRightHandTargetRifle;
    public TransformData ParentLeftHandTargetRifle;
    public TransformData RightArmRifle;
    public TransformData RightFistRifle;
    public TransformData LeftArmRifle;
    public TransformData LeftFistRifle;

    [Space(10)]


    [Header("Pistol")]
    public TransformData ParentRightHandTargetPistol;
    public TransformData ParentLeftHandTargetPistol;
    public TransformData RightArmPistol;
    public TransformData RightFistPistol;
    public TransformData LeftArmPistol;
    public TransformData LeftFistPistol;

    [Header("Bow")]
    public TransformData ParentRightHandTargetBow;
    public TransformData ParentLeftHandTargetBow;
    public TransformData RightArmBow;
    public TransformData RightFistBow;
    public TransformData LeftArmBow;
    public TransformData LeftFistBow;

}
