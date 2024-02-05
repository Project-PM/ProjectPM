using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ��� WindowUI�� ������ �̸��� ������ �ϸ�,
/// GameObject�� ������ �̸��� ������ �־�� �Ѵ�.
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
            Debug.Log("�˾� �ݱ� ����");
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
