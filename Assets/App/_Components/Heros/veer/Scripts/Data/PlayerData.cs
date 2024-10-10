using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData",menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float MovementVelocity = 4.5f;
    public float BackwardMovementVelocity = 2.8f;

    public float JumpMovementVelocity = 6.5f;
    public float JumpBackwardMovementVelocity = 4.5f;

    public float JumpForce = 800f;

    
}
