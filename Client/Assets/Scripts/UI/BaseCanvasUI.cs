using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class BaseCanvasUI : InitBase
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;

    protected virtual void Reset()
    {
        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();

        SetCanvas();
    }

    private void SetCanvas()
    {
        this.gameObject.layer = LayerMask.NameToLayer("UI");

        // Canvas 세팅
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 0;

        // CanvasScaler 세팅
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referencePixelsPerUnit = 100;
    }

    public void SetOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}

public class UIParam { }