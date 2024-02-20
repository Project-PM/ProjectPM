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

    private int currSortingOrder = 10;

    public BaseWindowUI OpenWindowUI(WindowUIType windowUIType, UIParam param = null)
    {
        return _currentMainWindow.OpenWindowUI(windowUIType, param);
    }

    public int PushPopupStack(BasePopupUI popupUI)
    {
        popupStack.Push(popupUI);

        return currSortingOrder++;
    }

    public void PopPopupStack()
    {
        popupStack.Pop();
        currSortingOrder--;
    }

    public bool ClosePopupUIEnsure(BasePopupUI popupUI)
    {
        if (popupStack.Count == 0)
            return true;

        if(popupStack.Peek() != popupUI)
        {
            Debug.Log($"팝업을 닫을 수 없음\n현재 최상위 팝업 : {popupStack.Peek().name}");
            return false;
        }

        return true;
    }

    public void Clear()
    {
        popupStack.Clear();
        CurrentMainWindow = null;
    }
}
