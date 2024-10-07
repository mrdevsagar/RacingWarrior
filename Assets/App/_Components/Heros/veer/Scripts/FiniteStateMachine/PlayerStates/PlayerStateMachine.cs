using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startingSate)
    {
        CurrentState = startingSate;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
    /*    Debug.Log(CurrentState.ToString());*/
        CurrentState.Enter();
    }

}
