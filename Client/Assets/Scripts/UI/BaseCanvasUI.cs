using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIParam { }

public class UIActiveEffectParam : UIParam
{
    public DirectionType dir;
    public RectTransform targetRectTr;
    public Image fadeImage;

    public UIActiveEffectParam(DirectionType dir, RectTransform targetRectTr, Image fadeImage)
    {
        this.dir = dir;
        this.targetRectTr = targetRectTr;
        this.fadeImage = fadeImage;
    }
}

public class BaseCanvasUI : InitBase
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        canvas = this.GetOrAddComponent<Canvas>();
        canvasScaler = this.GetOrAddComponent<CanvasScaler>();

        this.gameObject.layer = LayerMask.NameToLayer("UI");

        SetCanvas();
        SetCanvasScaler();
        SetResolution();

        return true;
    }

    private void SetCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.overrideSorting = true;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 0;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.vertexColorAlwaysGammaSpace = true;
    }

    protected void SetCanvasScaler()
    {
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = 100;
        canvasScaler.matchWidthOrHeight = 1.0f;
    }

    public void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

        OnPreCull();
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    protected void SetSortingOrder(int order)
    {
        canvas.sortingOrder = order;
    }

    void ActiveEffectPresetting(UIActiveEffectParam param)
    {

    }

    IEnumerator IActiveEffect(UIActiveEffectParam param)
    {
        // 초기 값

        yield return null;
    }
}

