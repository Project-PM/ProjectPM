using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum WindowUIType
{
    None,
    TestWindow1,
    TestWindow2,
}

public enum PopupUIType
{
    None,

}

public class UIManager
{
    private int _order = 10;

    private BaseMainWindow _currentMainWindow = null;
    public BaseMainWindow CurrentMainWindow
    {
        set { _currentMainWindow = value; }
        get { return _currentMainWindow; }
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        go.layer = LayerMask.NameToLayer("UI");

        Canvas canvas = Extension.GetOrAddComponent<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            cs.referencePixelsPerUnit = 100;
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = sortOrder;
        }
    }

    public BaseWindowUI OpenWindowUI(WindowUIType windowUIType)
    {
        return _currentMainWindow.OpenWindowUI(windowUIType);
    }
}
