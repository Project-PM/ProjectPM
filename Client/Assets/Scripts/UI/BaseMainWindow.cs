using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseMainWindow : BaseCanvasUI
{
    protected Dictionary<WindowUIType, BaseWindowUI> windowUIDic = new();

    public Stack<BasePopupUI> popupStack = new Stack<BasePopupUI>();
    BaseWindowUI currActiveWindow = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (this.gameObject.activeSelf == false)
            this.gameObject.SetActive(true);

        SetSortingOrder(1);

        Managers.UI.CurrentMainWindow = this;
        windowUIDic.Clear();
 
        Transform rootWindowsTr = this.transform.Find("@Windows");
        if (rootWindowsTr == null)
            return false;

        for (int i = 0; i < rootWindowsTr.childCount; i++)
        {
            if (rootWindowsTr.GetChild(i).TryGetComponent<BaseWindowUI>(out var baseWindowUI))
            {
                baseWindowUI.Init();
                windowUIDic.Add(baseWindowUI.WindowUIType, baseWindowUI);
            }
        }

        return true;
    }

    public virtual BaseWindowUI OpenWindowUI(WindowUIType windowUIType, UIParam param = null)
    {
        if (windowUIDic.ContainsKey(windowUIType) == false)
        {
            Debug.LogWarning($"{windowUIType} 는 현재 씬에 없습니다.");
            return null;
        }

        if (currActiveWindow != null && currActiveWindow.WindowUIType != windowUIType
            && currActiveWindow.IsActive)
            currActiveWindow.CloseWindowUI();
        
        currActiveWindow = windowUIDic[windowUIType];
        currActiveWindow.OpenWindowUI(param);
        return currActiveWindow;
    }
}
