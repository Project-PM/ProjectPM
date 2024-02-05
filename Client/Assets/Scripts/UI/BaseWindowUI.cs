using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindowUI : BaseCanvasUI
{
    public WindowUIType WindowUIType { get; protected set; }

    public bool IsActive { get; private set; } = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (this.gameObject.activeSelf == true)
            this.gameObject.SetActive(false);

        SetWindowUIType();

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

    /// <summary>
    /// 나의 windowUIType을 세팅
    /// </summary>
    protected abstract void SetWindowUIType();
}
