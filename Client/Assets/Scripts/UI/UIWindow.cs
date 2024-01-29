using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class UIWindow : MonoComponent<PrefabLinkedUISystem>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler scaler;

    protected override void Reset()
	{
		base.Reset();

        canvas = GetComponent<Canvas>();
		scaler = GetComponent<CanvasScaler>();

		SetCanvas();
		SetScaler();
		SetOrder(0);
	}

	private void Awake()
	{
		canvas.worldCamera = Camera.main;
		gameObject.layer = LayerMask.NameToLayer("UI");
	}

	private void SetCanvas()
	{
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
	}

	private void SetScaler()
	{
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1920, 1080);
		scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
		scaler.referencePixelsPerUnit = 100;
	}

	public void SetOrder(int orderIndex)
	{
		canvas.sortingOrder = LayerHelper.GetSortingLayer(gameObject, orderIndex);
	}
}
