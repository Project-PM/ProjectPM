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
        System.OnAttackInputChanged(keyType, isPressed);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
        System.OnAttackInputChanged(keyType, isPressed);
	}

	protected override void Update()
	{
		base.Update();

		if (isKeyboardMode)
		{
			System.OnAttackInputChanged(keyType, isPressed);
		}
	}
}
