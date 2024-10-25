using UnityEngine;

public class PlayerAliveState : PlayerState
{
    protected int JumpCount = 0;

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
        JumpCount = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        /* FlipPlayer();*/

        if ((MoveInput.x > 0 && player.transform.localScale.x > 0) || (MoveInput.x < 0 && player.transform.localScale.x < 0))
        {
            player.IsWalkingBackward = false;
        }
        else
        {
            player.IsWalkingBackward = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
       
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
   
  
    public override void OnJumpPress()
    {
        base.OnJumpPress();
        if (JumpCount <= 0)
        {
            player.RB.AddRelativeForceY(playerData.JumpForce,ForceMode2D.Impulse);
        }
    }

    /*    private void OnJump()
        {
            Debug.Log("jump!!!");
            player.RB.AddForce(new Vector2 (0, 20));
        }
    */


    #endregion
}
