using UnityEngine;
using UnityEngine.EventSystems;

public class GuardKeyComponent : InputKeyComponent
{
    protected override void Update()
    {
        base.Update();
	    System.OnGuardInputChanged(isPressed);
    }
}
