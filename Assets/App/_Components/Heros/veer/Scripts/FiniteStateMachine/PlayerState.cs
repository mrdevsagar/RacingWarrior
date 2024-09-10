using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animBoolName;

    protected Vector2 MoveInput;

    protected float LookInput;

    protected bool IsFiring;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        MoveInput = player.input.MoveInput;
        LookInput = player.input.LookInput;
        IsFiring = player.input.IsFiring;

        /*Debug.Log(MoveInput +"  "+ LookInput+"  " + IsFiring +"  "+LookInput);*/

        if ( MoveInput.x != 0 || !LookInput.Equals(float.NaN))
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
                if(player.transform.localScale.x < 0)
                {
                    player.FlipPlayer(true);
                }
               /* player.IsWalkingBackward = true;*/


            }
            else if (LookInput > 90 && LookInput < 270)
            {
                if (player.transform.localScale.x > 0)
                {
                    player.FlipPlayer(false);
                } 
             /*   player.IsWalkingBackward = true;*/
               
                
            }

            if ((MoveInput.x > 0 && player.transform.localScale.x > 0) || (MoveInput.x < 0 && player.transform.localScale.x < 0))
            {
                player.IsWalkingBackward = false;
            } else
            {
                player.IsWalkingBackward = true;
            }
        }
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() 
    {
        
    }
}
