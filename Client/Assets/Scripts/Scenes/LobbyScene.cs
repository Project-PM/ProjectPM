using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.Lobby;

        return true;
    }

    public override void Clear()
    {

    }
}
