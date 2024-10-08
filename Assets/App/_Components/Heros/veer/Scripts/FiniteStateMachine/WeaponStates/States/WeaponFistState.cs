using UnityEngine;

public class WeaponFistState : WeaponState
{

    public WeaponFistState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData) : base(player, playerStateMachine, playerData, weaponData)
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

    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
