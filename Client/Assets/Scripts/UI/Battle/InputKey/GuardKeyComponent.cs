using UnityEngine;
using UnityEngine.EventSystems;

public class GuardKeyComponent : InputKeyComponent
{
    private void Update()
    {
        System.OnGuardInputChanged(isPressed);
    }
}
