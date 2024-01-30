using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackKeyComponent : InputKeyComponent
{
    [SerializeField] private ENUM_ATTACK_KEY keyType = ENUM_ATTACK_KEY.MAX;

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		system.OnAttackInputChanged(keyType, isPressed, Time.frameCount);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
        system.OnAttackInputChanged(keyType, isPressed, Time.frameCount);
	}
}
