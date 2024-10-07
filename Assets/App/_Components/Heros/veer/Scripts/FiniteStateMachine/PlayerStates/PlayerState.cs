using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string _animBodyBoolName;

    private string _animLegsBoolName;

    protected Vector2 MoveInput;

    protected float LookInput;

    protected bool IsFiring;

    protected float LookDragDistance;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBodyBoolName, string animLegsBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        _animBodyBoolName = animBodyBoolName;
        _animLegsBoolName = animLegsBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();

        
            if (player.IsOverrideAnimation == false)
            {
                player.Anim.SetBool(_animBodyBoolName, true);
            }
            player.Anim.SetBool(_animLegsBoolName, true);

        

        startTime = Time.time;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(_animBodyBoolName, false);
        player.Anim.SetBool(_animLegsBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        MoveInput = player.input.MoveInput;
        LookInput = player.input.LookInput;
        IsFiring = player.input.IsFiring;
        LookDragDistance = player.input.LookDragDistance;
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

    public virtual void OnJumpPress()
    {

    }

    public virtual void SetCurrentBodyAnimation(bool isPlayAnimation)
    {
        player.Anim.SetBool(_animBodyBoolName, isPlayAnimation);
    }

    public virtual void SetCurrentLegsAnimation(bool isPlayAnimation)
    {
        player.Anim.SetBool(_animLegsBoolName, isPlayAnimation);
    }


}
