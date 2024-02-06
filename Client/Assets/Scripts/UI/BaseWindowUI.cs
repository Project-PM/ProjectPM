using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindowUI : BaseCanvasUI
{
    public WindowUIType WindowUIType { get; protected set; }

    public bool IsActive { get; private set; } = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (this.gameObject.activeSelf == true)
            this.gameObject.SetActive(false);

        SetSortingOrder(5);

        WindowUIType = Extension.ParseEnum<WindowUIType>(this.gameObject.name);
        
        if (WindowUIType == WindowUIType.None)
            Debug.LogError("WindowUIType 열거형과 GameObject 이름이 다릅니다.");

        return true;
    }

    public virtual bool OpenWindowUI(UIParam param = null)
    {
        if (IsActive)
            return false;

        IsActive = true;
        this.gameObject.SetActive(true);

        return true;
    }
    
    public virtual bool CloseWindowUI()
    {
        if (!IsActive)
            return false;

        IsActive = false;
        this.gameObject.SetActive(false);

        return true;
    }

    #region OnClick Event
    public void OnClickExitWindow()
    {
        CloseWindowUI();
    }
    #endregion
}
