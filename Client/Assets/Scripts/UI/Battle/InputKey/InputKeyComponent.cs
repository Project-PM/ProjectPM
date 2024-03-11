using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputKeyComponent : MonoComponent<FrameInputSystem>, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField] protected bool isKeyboardMode = false;
	[SerializeField] private KeyCode KeyBoardCode = KeyCode.None;

	protected bool isPressed = false;

    public virtual void OnDrag(PointerEventData eventData)
	{
		isPressed = true;
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		isPressed = false;
	}

	protected virtual void Update()
	{
		if (isKeyboardMode)
		{
			isPressed = Input.GetKey(KeyBoardCode);
		}
	}
}
