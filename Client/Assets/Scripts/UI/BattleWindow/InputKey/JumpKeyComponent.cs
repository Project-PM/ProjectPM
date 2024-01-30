using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpKeyComponent : InputKeyComponent
{
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        System.OnJumpInputChanged(isPressed, Time.frameCount);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        System.OnJumpInputChanged(isPressed, Time.frameCount);
    }
}
