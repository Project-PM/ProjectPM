using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 모든 WindowUI는 고유한 이름을 가져야 하며,
/// GameObject와 동일한 이릉을 가지고 있어야 한다.
/// </summary>
public enum WindowUIType
{
    None,

    // TitleWindow

    // LobbyWindow
    PlayerProfileWindow,


    // BattleWindow
}

public class UIManager
{
    private BaseMainWindow _currentMainWindow = null;
    public BaseMainWindow CurrentMainWindow
    {
        set { _currentMainWindow = value; }
        get { return _currentMainWindow; }
    }

    Stack<BasePopupUI> popupStack = new Stack<BasePopupUI>();

    private int currPopupOrder = 10;

    public BaseWindowUI OpenWindowUI(WindowUIType windowUIType, UIParam param = null)
    {
        return _currentMainWindow.OpenWindowUI(windowUIType, param);
    }

    public void PushPopupStack(BasePopupUI popupUI)
    {
        popupStack.Push(popupUI);
    }

    public void ClosePopupUI(BasePopupUI popupUI)
    {
        if (popupStack.Count == 0)
            return;

        if(popupStack.Peek() != popupUI)
        {
            Debug.Log("팝업 닫기 실패");
            return;
        }
        
        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }

    private void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        BasePopupUI currPopupUI = popupStack.Pop();

        currPopupUI.ClosePopupUI();
        currPopupOrder--;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        CurrentMainWindow = null;
    }
}
