using UnityEngine;

public class WeaponFistState : WeaponState
{

    public WeaponFistState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData, GameObject weaponObject) : base(player, playerStateMachine, playerData, weaponData, weaponObject)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetFloat("BlendIdle", 0);
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

        if (player.input.IsFiring)
        {
            AttackFist();
        }
    }

    public override void AttackWeapon()
    {
        base.AttackWeapon();
        AttackFist();
    }


    public void AttackFist()
    {
        playerStateMachine.CurrentState.SetCurrentBodyAnimation(false);
        player.Anim.SetBool("PunchAttack", true);
    }
}
