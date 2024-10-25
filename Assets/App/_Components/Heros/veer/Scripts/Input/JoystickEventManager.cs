using UnityEngine;
using System;

public static class JoystickEventManager
{
    // Events to broadcast the changes
    public static event Action<bool> OnFiringChanged;
    public static event Action<float> OnDistanceChanged;
    public static event Action<float> OnAngleChanged;

    // Static methods to invoke events without needing an object reference
    public static void TriggerFiringChanged(bool isFiring)
    {
        OnFiringChanged?.Invoke(isFiring);
    }

    public static void TriggerDistanceChanged(float distance)
    {
        OnDistanceChanged?.Invoke(distance);
    }

    public static void TriggerAngleChanged(float angle)
    {
        OnAngleChanged?.Invoke(angle);
    }
}
