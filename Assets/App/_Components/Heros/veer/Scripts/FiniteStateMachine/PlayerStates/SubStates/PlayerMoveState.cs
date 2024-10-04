using TMPro;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName) : base(player, stateMachine, playerData, animBodyBoolName, animLegsBoolName)
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

        if (MoveInput.x == 0f)
        {
          /*  if (player.GetComponent<Rigidbody2D>().velocityX == 0f)*/
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.IsWalkingBackward)
        {
            player.SetVelocityX(MoveInput.x * playerData.BackwardMovementVelocity);
        }
        else
        {
            player.SetVelocityX(MoveInput.x * playerData.MovementVelocity);
        }

    }

    
}
