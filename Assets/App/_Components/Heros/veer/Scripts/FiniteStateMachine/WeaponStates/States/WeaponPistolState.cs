using UnityEngine;

public class WeaponPistolState : WeaponState
{
    public WeaponPistolState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData, GameObject weaponObject) : base(player, playerStateMachine, playerData, weaponData, weaponObject)
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


        player.Comp.Rifle.P_AKM_Parent_RightHandTarget.transform.parent = player.Comp.Body.Targets;

        player.Comp.Pistol.P_Revolver_LeftArmTarget.transform.parent = player.Comp.Pistol.PistolGameObj.transform;
        player.Comp.Pistol.P_Revolver_LeftFistTarget.transform.parent = player.Comp.Pistol.PistolGameObj.transform;

        player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.localPosition = weaponData.ParentRightHandTargetPistol.position;

        player.Comp.Pistol.P_Revolver_RightArmTarget.transform.localPosition = weaponData.RightArmPistol.position;
        player.Comp.Pistol.P_Revolver_RightFistTarget.transform.localPosition = weaponData.RightFistPistol.position;


        player.Comp.Pistol.P_Revolver_LeftArmTarget.transform.localRotation = Quaternion.identity;
        player.Comp.Pistol.P_Revolver_LeftFistTarget.transform.localRotation = Quaternion.identity;

        player.Comp.Pistol.P_Revolver_LeftArmTarget.transform.localPosition = weaponData.LeftArmPistol.position;
        player.Comp.Pistol.P_Revolver_LeftFistTarget.transform.localPosition = weaponData.LeftFistPistol.position;
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

        RevolverAim();
    }

   


    public void RevolverAim()
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


    protected override void SetHandsIKSolvers()
    {
        player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_LeftArmTarget;
        player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_LeftFistTarget;

        player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_RightArmTarget;
        player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_RightFistTarget;
    }


    private void RotateRightHand(float handRotationAngle)
    {
        player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.eulerAngles = new Vector3(player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.rotation.x, player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);
    }

}
