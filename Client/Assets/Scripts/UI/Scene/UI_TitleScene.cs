using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_TitleScene : UI_BaseScene
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
        // PreLoad 라벨이 붙은 모든 리소스를 로드하고 콜백함수로 완료 통보를 받음
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();

                // 씬이동이 가능한 오브젝트 활성화시키기?
            }
        });
    }
}
