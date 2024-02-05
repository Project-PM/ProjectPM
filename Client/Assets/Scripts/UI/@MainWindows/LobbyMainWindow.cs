using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMainWindow : BaseMainWindow
{
    [SerializeField] BasePopupUI popupUI;


    public void TestButton()
    {
        popupUI.OpenPopupUI();
    }
}
