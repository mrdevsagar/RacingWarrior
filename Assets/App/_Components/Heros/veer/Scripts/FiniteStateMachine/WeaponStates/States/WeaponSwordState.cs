using UnityEngine;
using UnityEngine.Windows;

public class WeaponSwordState : WeaponState
{
    public WeaponSwordState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData) : base(player, playerStateMachine, playerData, weaponData)
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
        SwordMovement();
    }

    public override void AttackWeapon()
    {
        base.AttackWeapon();
        AttackSword();
    }

    public void SwordMovement()
    {
        if (player.input.IsFiring)
        {
            AttackSword();
        }
    }

    public void AttackSword()
    {
        playerStateMachine.CurrentState.SetCurrentBodyAnimation(false);
        player.Anim.SetBool("SordAttck", true);
    }
}
