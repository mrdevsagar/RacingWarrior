using UnityEngine;

public class WeaponGlovesState : WeaponState
{
    public WeaponGlovesState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData) : base(player, playerStateMachine, playerData, weaponData)
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
