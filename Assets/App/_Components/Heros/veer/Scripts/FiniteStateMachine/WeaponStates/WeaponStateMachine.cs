using UnityEngine;

public class WeaponStateMachine 
{
    public WeaponState CurrentWeaponState { get; private set; }

    public void Initialize(WeaponState startingSate)
    {
        CurrentWeaponState = startingSate;
        CurrentWeaponState.Enter();
    }

    public void ChangeState(WeaponState newState)
    {
        CurrentWeaponState.Exit();
        CurrentWeaponState = newState;
        Debug.Log("Selected Weapon State: "+ CurrentWeaponState.ToString());
        CurrentWeaponState.Enter();
    }
}
