using UnityEngine;
using UnityEngine.EventSystems;

public class GuardKeyComponent : InputKeyComponent
{
	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
        System.OnGuardInputChanged(isPressed, Time.frameCount);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
        System.OnGuardInputChanged(isPressed, Time.frameCount);
	}
}
