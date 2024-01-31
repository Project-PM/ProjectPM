using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMainWindow : BaseMainWindow
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        

        return true;
    }

    public void OnClickTest1()
    {
        Managers.UI.OpenWindowUI(WindowUIType.TestWindow1);
    }

    public void OnClickTest2()
    {
        Managers.UI.OpenWindowUI(WindowUIType.TestWindow2);
    }
}
