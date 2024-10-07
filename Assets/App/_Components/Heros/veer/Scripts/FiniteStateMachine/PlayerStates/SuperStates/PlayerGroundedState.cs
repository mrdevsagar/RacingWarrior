using UnityEngine;

public class PlayerGroundedState : PlayerAliveState
{

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName) : base(player, stateMachine, playerData, animBodyBoolName, animLegsBoolName)
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

        if (!player.IsTouchingGround())
        {
            stateMachine.ChangeState(player.InAirState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
    }

    private bool IsGrounded()
    {
        return true;
    }
}
