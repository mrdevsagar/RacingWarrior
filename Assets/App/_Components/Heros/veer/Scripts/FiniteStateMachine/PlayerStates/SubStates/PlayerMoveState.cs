using TMPro;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
/*
        Debug.Log(MoveInput);*/
        if (MoveInput.x == 0)
        {
            player.SetFrictionMaterial(false);
        } else
        {
            player.SetFrictionMaterial(true);

            if (!player.IsWalkingBackward)
            {
                player.SetVelocityX(MoveInput.x * playerData.MovementVelocity);
            } else
            {
                player.SetVelocityX(MoveInput.x * playerData.BackwardMovementVelocity);
            }
        }
    }
}
