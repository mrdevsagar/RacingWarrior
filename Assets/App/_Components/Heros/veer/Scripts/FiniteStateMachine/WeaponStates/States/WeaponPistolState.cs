using UnityEngine;

public class WeaponPistolState : WeaponState
{
    public WeaponPistolState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData) : base(player, playerStateMachine, playerData, weaponData)
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



        if (handRotationAngle.Equals(float.NaN))
        {
            player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Body.AnimLeftArmTarget;
            player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Body.AnimLeftFistTarget;

            player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Body.AnimRightArmTarget;
            player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Body.AnimRightFistTarget;

            player.Comp.Body.Head.eulerAngles = new Vector3(player.Comp.Body.Head.eulerAngles.x, player.Comp.Body.Head.eulerAngles.y, 90 * (player.IsPlayerLeftFacing ? -1 : 1));


        }
        else
        {
            if (player.IsPlayerLeftFacing)
            {
                handRotationAngle += player.flipAngle;
            }

            player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_LeftArmTarget;
            player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_LeftFistTarget;

            player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_RightArmTarget;
            player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Pistol.P_Revolver_RightFistTarget;

            player.Comp.Pistol.P_Revolver_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(player.Comp.Pistol.P_Revolver_Parent_LeftHandTarget.transform.rotation.x, player.Comp.Pistol.P_Revolver_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

            player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.eulerAngles = new Vector3(player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.rotation.x, player.Comp.Pistol.P_Revolver_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

            player.RotateHead(angle);


        }
    }


}
