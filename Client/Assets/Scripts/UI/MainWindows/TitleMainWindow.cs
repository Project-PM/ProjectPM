using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMainWindow : BaseMainWindow
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartLoadAssets();

        return true;
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();
            }
        });
    }
}
