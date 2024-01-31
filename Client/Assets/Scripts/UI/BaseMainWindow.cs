using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseMainWindow : InitBase
{
    protected Dictionary<WindowUIType, BaseWindowUI> _windowUIDic = new();

    protected virtual void Start()
    {
        _windowUIDic.Clear();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            BaseWindowUI baseWindowUI = this.transform.GetChild(i).GetComponent<BaseWindowUI>();

            if (baseWindowUI != null)
            {
                baseWindowUI.Init();
                _windowUIDic.Add(baseWindowUI.WindowUIType, baseWindowUI);
                Debug.Log($"{baseWindowUI.name}, {baseWindowUI.WindowUIType}");
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);
        Managers.UI.CurrentMainWindow = this;

        return true;
    }

    public virtual BaseWindowUI OpenWindowUI(WindowUIType windowUIType)
    {
        if (_windowUIDic.ContainsKey(windowUIType) == false)
        {
            Debug.LogWarning($"{windowUIType} 는 현재 씬에 없습니다.");
            return null;
        }

        _windowUIDic[windowUIType].OpenUI();
        return _windowUIDic[windowUIType];
    }
}
