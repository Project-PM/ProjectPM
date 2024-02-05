using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvasUI : InitBase
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;

    protected virtual void Reset()
    {
        canvas = this.GetOrAddComponent<Canvas>();
        canvasScaler = this.GetOrAddComponent<CanvasScaler>();

        this.gameObject.layer = LayerMask.NameToLayer("UI");
        SetCanvas();
        SetCanvasScaler();
    }

    private void SetCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 0;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.vertexColorAlwaysGammaSpace = true;
    }

    protected void SetCanvasScaler()
    {
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referencePixelsPerUnit = 100;
    }

    protected void SetSortingOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}

public class UIParam { }