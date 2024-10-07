using UnityEngine;

public class PlayerInAirState : PlayerAliveState
{
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName) : base(player, stateMachine, playerData, animBodyBoolName, animLegsBoolName)
    {
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

        
        if (player.IsWalkingBackward)
        {
            player.SetVelocityX(MoveInput.x * playerData.JumpBackwardMovementVelocity);
        }
        else
        {
            player.SetVelocityX(MoveInput.x * playerData.JumpMovementVelocity);
        }
    }

    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
    }

    public override void OnJumpPress()
    {
        base.OnJumpPress();
        JumpCount++;
    }

    public override void DoChecks()
    {
        base.DoChecks();

       
        if (player.IsTouchingGround())
        {
            if (MoveInput.x == 0f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (MoveInput.x != 0f)
            {
                stateMachine.ChangeState(player.MoveState);
            }

        }
    }
}
