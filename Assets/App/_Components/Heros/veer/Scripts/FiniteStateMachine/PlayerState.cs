using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string _animBoolName;

    protected Vector2 MoveInput;

    protected float LookInput;

    protected bool IsFiring;

    protected float LookDragDistance;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(_animBoolName, true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(_animBoolName, false);
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

    public virtual void LatePhysicsUpdate()
    {

    }


    public virtual void DoChecks() 
    {
        
    }
}
