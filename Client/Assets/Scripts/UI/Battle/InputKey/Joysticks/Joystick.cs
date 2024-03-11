using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AxisOptions 
{ 
	Both, 
	Horizontal, 
	Vertical 
}

public class Joystick : InputKeyComponent
{
	[SerializeField] private float deadZone = 0;
	[SerializeField] private float handleRange = 1;

	[SerializeField] public AxisOptions AxisOptions = AxisOptions.Both;
	[SerializeField] private bool SnapX = true;
	[SerializeField] private bool SnapY = true;

	[SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;

	private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;

    protected virtual void Start()
    {
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        Vector2 center = new Vector2(0.5f, 0.5f);

        background.pivot = center;

        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        cam = null;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
		{
			cam = canvas.worldCamera;
		}

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;

        input = (eventData.position - position) / (radius * canvas.scaleFactor);

        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, cam);

		handle.anchoredPosition = input * radius * handleRange;
    }

	protected override void Update()
	{
		float x = 0.0f;
		float y = 0.0f;

		if (isKeyboardMode)
		{
			x = Input.GetAxisRaw("Horizontal");
			y = Input.GetAxisRaw("Vertical");
		}
		else
		{
			x = SnapX ? SnapFloat(input, input.x, AxisOptions.Horizontal) : input.x;
			y = SnapY ? SnapFloat(input, input.y, AxisOptions.Vertical) : input.y;
		}

		System.OnMoveInputChanged(new Vector2(x, y));
	}

	protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
				input = normalised;
			}
		}
        else
        {
			input = Vector2.zero;
		}
	}

    private void FormatInput()
    {
        if (AxisOptions == AxisOptions.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (AxisOptions == AxisOptions.Vertical)
            input = new Vector2(0f, input.y);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
	}

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out Vector2 localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

	private float SnapFloat(Vector2 input, float value, AxisOptions snapAxis)
	{
		if (value == 0)
			return value;

		if (AxisOptions == AxisOptions.Both)
		{
			float angle = Vector2.Angle(input, Vector2.up);
			if (snapAxis == AxisOptions.Horizontal)
			{
				if (angle < 22.5f || angle > 157.5f)
				{
					return 0;
				}
				else
				{
					return (value > 0) ? 1 : -1;
				}
			}
			else if (snapAxis == AxisOptions.Vertical)
			{
				if (angle > 67.5f && angle < 112.5f)
				{
					return 0;
				}
				else
				{
					return (value > 0) ? 1 : -1;
				}
			}
			else
			{
				return value;
			}
		}
		else
		{
			if (value > 0)
			{
				return 1;
			}
			else if (value < 0)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
	}

}

