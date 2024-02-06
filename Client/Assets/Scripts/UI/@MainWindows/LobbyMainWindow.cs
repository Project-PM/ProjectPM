using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMainWindow : BaseMainWindow
{


    #region OnClick Event
    public void OnClickPlayerProfile()
    {
        OpenWindowUI(WindowUIType.PlayerProfileWindow);
    }
    #endregion
}
