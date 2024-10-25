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

    protected float Distance;
    protected float Angle;
    protected bool IsFiring;
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

        OnEnable();
    }

    public virtual void Exit()
    {
        if (weaponObject != null)
        { 
            weaponObject.SetActive(false); 
        }

        OnDisable();
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
        CameraAim();
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

    private void CameraAim()
    {
        float angle = player.input.LookInput;

        float handRotationAngle = angle;



        if (!handRotationAngle.Equals(float.NaN))
        {
            if (player.IsPlayerLeftFacing)
            {
                handRotationAngle += player.flipAngle;
            }

           
            player.PlayerCameraHolder.eulerAngles = new Vector3(player.PlayerCameraHolder.rotation.x, player.PlayerCameraHolder.rotation.y, handRotationAngle);
        }
    }

    private void OnEnable()
    {
        JoystickEventManager.OnFiringChanged += HandleFiringChanged;
        JoystickEventManager.OnDistanceChanged += HandleDistanceChanged;
        JoystickEventManager.OnAngleChanged += HandleAngleChanged;
    }

    private void OnDisable()
    {
        JoystickEventManager.OnFiringChanged -= HandleFiringChanged;
        JoystickEventManager.OnDistanceChanged -= HandleDistanceChanged;
        JoystickEventManager.OnAngleChanged -= HandleAngleChanged;
    }

    protected virtual void HandleFiringChanged(bool isFiring)
    {
        IsFiring = isFiring;
    }

    protected virtual void HandleDistanceChanged(float distance)
    {
        Distance = distance;
    }



    protected virtual void HandleAngleChanged(float angle)
    {
        if (angle > 90 && angle < 270)
        {
            player.FlipPlayer(false);
        }
        else
        {
            player.FlipPlayer(true);
        }
        Angle = angle;
    } 


}