using UnityEngine;

public class WeaponRifleState : WeaponState
{
    public WeaponRifleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData,GameObject weaponObject) : base(player, playerStateMachine, playerData, weaponData, weaponObject)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetFloat("BlendIdle", 2);

        
    }

    public override void Exit()
    {
        base.Exit();

        RotateRightHand(0);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
        RifleAim();
    }

   


    public void RifleAim()
    {
        float angle = player.input.LookInput;

        float handRotationAngle = angle;



        if (!handRotationAngle.Equals(float.NaN))
        {
            if (player.IsPlayerLeftFacing)
            {
                handRotationAngle += player.flipAngle;
            }

            RotateRightHand(handRotationAngle);
            player.RotateHead(angle);
        }
    }

    private void RotateRightHand(float handRotationAngle)
    {
        player.Comp.Rifle.P_AKM_Parent_RightHandTarget.transform.eulerAngles = new Vector3(player.Comp.Rifle.P_AKM_Parent_RightHandTarget.transform.rotation.x, player.Comp.Rifle.P_AKM_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);
    }

    protected override void SetHandsIKSolvers()
    {
        player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Rifle.P_AKM_LeftArmTarget;
        player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Rifle.P_AKM_LeftFistTarget;

        player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Rifle.P_AKM_RightArmTarget;
        player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Rifle.P_AKM_RightFistTarget;
    }


   

    protected override void HandleAngleChanged(float angle)
    {
        base.HandleAngleChanged(angle);

        if (angle > 90 && angle < 270)
        {
            RotateRightHand(angle - 180);
        } else
        {
            RotateRightHand(angle);
        }
        player.RotateHead(angle);
    }
}
