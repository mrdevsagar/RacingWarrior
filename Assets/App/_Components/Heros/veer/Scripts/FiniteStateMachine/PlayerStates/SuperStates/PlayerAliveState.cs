using UnityEngine;

public class PlayerAliveState : PlayerState
{
    public PlayerAliveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName) : base(player, stateMachine, playerData, animBodyBoolName, animLegsBoolName)
    {
    }
    #region State Machine Override Methods
   

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
        FlipPlayer();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void LatePhysicsUpdate()
    {
        base.LatePhysicsUpdate();
        RotateHand();
    }



    #endregion

    #region Other Methods
    private void FlipPlayer()
    {
        if (MoveInput.x != 0 || !LookInput.Equals(float.NaN))
        {
            if (MoveInput.x > 0 && player.transform.localScale.x < 0 && LookInput.Equals(float.NaN))
            {
                player.FlipPlayer(true);
            }
            else if (MoveInput.x < 0 && player.transform.localScale.x > 0 && LookInput.Equals(float.NaN))
            {
                player.FlipPlayer(false);
            }

            if (((LookInput >= 0 && LookInput <= 90) || (LookInput >= 270 && LookInput <= 360)))
            {
                if (player.transform.localScale.x < 0)
                {
                    player.FlipPlayer(true);
                }
            }
            else if (LookInput > 90 && LookInput < 270)
            {
                if (player.transform.localScale.x > 0)
                {
                    player.FlipPlayer(false);
                }
            }

            // Enabling and disabling backward movement.
            if ((MoveInput.x > 0 && player.transform.localScale.x > 0) || (MoveInput.x < 0 && player.transform.localScale.x < 0))
            {
                player.IsWalkingBackward = false;
            }
            else
            {
                player.IsWalkingBackward = true;
            }
        }
    }
   
    private void RotateHand()
    {
        if (player.SelectedWeapon.Equals(Weapon.AKM))
        {
            player.RifleAim();
        }

        switch (player.SelectedWeapon)
        {
            case Weapon.AKM: 
                player.RifleAim();                  
                break;

            case Weapon.BOW:
                player.BowAim();
                break;

            case Weapon.Revolver:
                player.RevolverAim();
                break;

            case Weapon.SWORD:
                player.SwordAttack();
                break;

            default : break;
        }


        /* player.MoveBowRightHand(LookInput,LookDragDistance);*/
    }

    #endregion
}
