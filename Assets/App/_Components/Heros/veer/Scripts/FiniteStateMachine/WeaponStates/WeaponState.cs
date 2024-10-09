using UnityEngine;

public class WeaponState
{
    protected WeaponStateMachine WeaponStateMachine;
    protected float startTime;

    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;
    protected WeaponData weaponData;
    protected GameObject weaponObject;
    public WeaponState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData,GameObject weaponObject)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerData = playerData;
        this.weaponData = weaponData;
        this.weaponObject = weaponObject;
    }
    public virtual void Enter()
    {
        DoChecks();
        if (weaponObject != null) 
        { 
            weaponObject.SetActive(true);
        }
        SetHandsIKSolvers();
        RestHeadRotation();
    }

    public virtual void Exit()
    {
        if (weaponObject != null)
        { 
            weaponObject.SetActive(false); 
        }

        
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

    protected virtual void SetHandsIKSolvers()
    {
        player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Body.AnimLeftArmTarget;
        player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Body.AnimLeftFistTarget;

        player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Body.AnimRightArmTarget;
        player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Body.AnimRightFistTarget;

        
    }
    
     protected void RestHeadRotation()
    {
        player.Comp.Body.Head.eulerAngles = new Vector3(player.Comp.Body.Head.eulerAngles.x, player.Comp.Body.Head.eulerAngles.y, 90 * (player.IsPlayerLeftFacing ? -1 : 1));
    }
}