using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindowUI : MonoBehaviour
{
    public WindowUIType WindowUIType { get; protected set; }
    protected bool _init = false;
    
    public virtual bool Init()
    {
        if (_init)
            return false;

        SetWindowUIType();

        _init = true;
        return true;
    }

    protected virtual void Awake()
    {
        Init();
    }

    public virtual void OpenUI()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void CloseUI()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 나의 windowUIType을 세팅
    /// </summary>
    protected abstract void SetWindowUIType();
}
