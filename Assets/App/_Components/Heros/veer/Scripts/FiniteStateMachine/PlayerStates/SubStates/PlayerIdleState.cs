using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{

    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName) : base(player, stateMachine, playerData, animBodyBoolName, animLegsBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.physicsMaterial.friction = 8f;
        player.PlayerCollider.sharedMaterial = player.physicsMaterial;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (MoveInput.x != 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

   
}
