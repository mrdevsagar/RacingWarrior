using UnityEngine;

public class WeaponState
{
    protected WeaponStateMachine WeaponStateMachine;
    protected float startTime;

    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;
    protected WeaponData weaponData;
    public WeaponState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerData = playerData;
        this.weaponData = weaponData;
    }
    public virtual void Enter()
    {
        DoChecks();
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void PhysicsLateUpdate()
    {

    }
    public virtual void DoChecks()
    {

    }

    public virtual void AttackWeapon()
    {

    }
}