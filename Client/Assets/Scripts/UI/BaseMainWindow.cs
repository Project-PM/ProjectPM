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

    protected virtual void Start()
    {
        windowUIDic.Clear();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            BaseWindowUI baseWindowUI = this.transform.GetChild(i).GetComponent<BaseWindowUI>();

            if (baseWindowUI != null)
            {
                baseWindowUI.Init();
                windowUIDic.Add(baseWindowUI.WindowUIType, baseWindowUI);
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.CurrentMainWindow = this;

        if (this.gameObject.activeSelf == false)
            this.gameObject.SetActive(true);

        return true;
    }

    public virtual BaseWindowUI OpenWindowUI(WindowUIType windowUIType, UIParam param = null)
    {
        if (windowUIDic.ContainsKey(windowUIType) == false)
        {
            Debug.LogWarning($"{windowUIType} 는 현재 씬에 없습니다.");
            return null;
        }

        if (currActiveWindow != null && currActiveWindow.WindowUIType != windowUIType)
            currActiveWindow.CloseWindowUI();
        
        currActiveWindow = windowUIDic[windowUIType];
        currActiveWindow.OpenWindowUI(param);
        return currActiveWindow;
    }
}
