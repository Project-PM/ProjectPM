using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.Title;

        // StartLoadAssets();

        return true;
    }

    public override void Clear()
    {

    }
}
