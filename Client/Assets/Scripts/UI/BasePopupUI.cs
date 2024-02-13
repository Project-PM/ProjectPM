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
        int sortingOrder = Managers.UI.PushPopupStack(this);
        SetSortingOrder(sortingOrder);
        this.gameObject.SetActive(true);

        return true;
    }

    public virtual bool ClosePopupUI()
    {
        if (!IsActive)
            return false;

        if (Managers.UI.ClosePopupUIEnsure(this) == false)
            return false;

        IsActive = false;
        Managers.UI.PopPopupStack();
        this.gameObject.SetActive(false);

        return true;
    }
}
