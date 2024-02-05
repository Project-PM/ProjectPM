using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopupUI : BaseCanvasUI
{
    public bool IsActive { get; private set; } = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (this.gameObject.activeSelf == true)
            this.gameObject.SetActive(false);

        return true;
    }

    public virtual bool OpenPopupUI(UIParam param = null)
    {
        if (IsActive)
            return false;

        IsActive = true;
        this.gameObject.SetActive(true);

        Managers.UI.PushPopupStack(this);

        return true;
    }

    public virtual bool ClosePopupUI()
    {
        if (!IsActive)
            return false;

        IsActive = false;
        this.gameObject.SetActive(false);

        return true;
    }
}
