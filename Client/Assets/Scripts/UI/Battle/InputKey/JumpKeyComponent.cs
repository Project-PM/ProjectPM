using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpKeyComponent : InputKeyComponent
{
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        System.OnJumpInputChanged(isPressed);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        System.OnJumpInputChanged(isPressed);
    }

	protected override void Update()
	{
        base.Update();

        if (isKeyboardMode)
        {
			System.OnJumpInputChanged(isPressed);
		}
	}
}
